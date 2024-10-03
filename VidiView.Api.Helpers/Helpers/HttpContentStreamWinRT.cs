#if WINRT
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Versioning;
using VidiView.Api.Exceptions;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace VidiView.Api.Helpers;

/// <summary>
/// This class represents a read-only, seekable Http stream
/// </summary>
/// <remarks>This stream buffers the last response. 
/// Seeks may cause a round trip to the server</remarks>
[SupportedOSPlatform("windows10.0.17763.0")]
public sealed class HttpContentStreamWinRT : IRandomAccessStreamWithContentType, IProgress<HttpProgress>, IProgress<ulong>
{
    public static IAsyncOperationWithProgress<HttpContentStreamWinRT, ulong> CreateFromUriAsync(HttpClient httpClient, Uri uri)
    {
        return CreateFromUriAsync(httpClient, uri, null);
    }

    public static IAsyncOperationWithProgress<HttpContentStreamWinRT, ulong> CreateFromUriAsync(HttpClient httpClient, Uri uri, uint? maxRequestSize)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(uri, nameof(uri));

        return AsyncInfo.Run<HttpContentStreamWinRT, ulong>(async (token, progress) =>
        {

            var stream = new HttpContentStreamWinRT(httpClient, uri)
            {
                MaxRequestSize = maxRequestSize,
            };

            await stream.InitializeAsync(progress);
            return stream;
        });
    }

    private readonly HttpClient _httpClient;
    private readonly Uri _requestUri;

    private ulong _size;
    private bool _isDisposed;

    private IProgress<ulong>? _progress;
    private ulong _progressOffset = 0;

    private ContentInfo? _cachedResponse;

    public HttpContentStreamWinRT(HttpClient httpClient, Uri uri)
    {
        _httpClient = httpClient;
        _requestUri = uri;
    }

    /// <summary>
    /// Call to perform an initial read and determine <see cref="ContentType"/> and <see cref="Size"/>
    /// </summary>
    /// <param name="progress">
    /// If supplied, this instance will receive progress events during download.
    /// The reported progress value is the current position of the stream (0 <= value <= Size)
    /// </param>
    /// <returns></returns>
    public async Task InitializeAsync(IProgress<ulong>? progress = null)
    {
        _progress = progress;

        _progress?.Report(0);
        _cachedResponse = await ExecuteReadAsync(0, MaxRequestSize, CancellationToken.None);
        CanRead = true;
    }

    /// <summary>
    /// Get/set maximum request size. This affects the 
    /// number of bytes requested from the server in an 
    /// Http get request
    /// </summary>
    /// <remarks>
    /// Set to null, to read the maximum allowed size 
    /// from the server
    /// </remarks>
    public uint? MaxRequestSize { get; set; } = null;

    /// <summary>
    /// The stream content type
    /// </summary>
    public string? ContentType { get; private set; }
    public bool CanRead { get; private set; } = false;
    public bool CanWrite => false;
    public ulong Position { get; private set; }
    public ulong Size { get => _size; set => throw new NotSupportedException(); }

    /// <summary>
    /// Number of read requests executed
    /// </summary>
    public int HttpGetRequestCount { get; private set; }

    /// <summary>
    /// Returns the number of bytes currently read by this reader (including repeats etc)
    /// </summary>
    public long HttpBytesRead { get; private set; }

    public IRandomAccessStream CloneStream()
    {
        if (_isDisposed)
            throw new ObjectDisposedException(nameof(HttpContentStreamWinRT));

        return new HttpContentStreamWinRT(_httpClient, _requestUri)
        {
            _size = _size,
            _progress = _progress,
            ContentType = ContentType,
            Position = Position,
            _cachedResponse = _cachedResponse
        };
    }

    public IInputStream GetInputStreamAt(ulong position)
    {
        var clone = CloneStream();
        clone.Seek(position);
        return clone;
    }

    public void Seek(ulong position)
    {
        // No validation here
        Position = position;
    }

    public IAsyncOperationWithProgress<IBuffer, uint> ReadAsync(IBuffer destination, uint count, InputStreamOptions options)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        if (!CanRead)
            throw new InvalidOperationException("Stream CanRead = False");

        return AsyncInfo.Run<IBuffer, uint>(async (token, progress) =>
        {
            uint destinationOffset = 0;
            do
            {
                var response = _cachedResponse ?? throw new InvalidOperationException("No response in cache. Disposed?");

                if (response.PositionWithinBuffer(Position, out var sourceOffset) != true)
                {
                    if (Position < Size)
                    {
                        response = await ExecuteReadAsync(Position, MaxRequestSize, CancellationToken.None);
                        _cachedResponse = response;
                        sourceOffset = 0;

                    }
                    else
                    {
                        count = 0;
                        sourceOffset = 0;
                    }
                    // We don't have the requested position in our cached buffer
                }

                var bytesToCopy = Math.Min(count, response.Buffer!.Length - sourceOffset);

#if false
// It appears that M$ themselves doesn't respect the fact that
// the returned buffer should be used, i.e. when copying streams
// using RandomAccessStream.CopyAsync()

                if (sourceOffset == 0 
                    && destinationOffset == 0
                    && (response.Buffer.Length >= count || options.HasFlag(InputStreamOptions.Partial)))
                {
                    // We can just return the source buffer as is,
                    // without any unnecessary copying
                    Position += bytesToCopy;
                    return response.Buffer;
                }
#endif

                // Copy from source buffer to destination buffer

                // https://github.com/microsoft/CsWinRT/issues/1808
                // response.Buffer.CopyTo(sourceOffset, destination, destinationOffset, bytesToCopy);
                WorkaroundCopy(response.Buffer, sourceOffset, destination, destinationOffset, bytesToCopy);

                destinationOffset += bytesToCopy;
                destination.Length = destinationOffset;

                Position += bytesToCopy;
                count -= bytesToCopy;

                if (options.HasFlag(InputStreamOptions.Partial))
                    break;

            } while (count > 0 && Position < Size);

            // Return the data
            return destination;
        });
    }

    static void WorkaroundCopy(IBuffer sourceBuffer, uint sourceOffset, IBuffer destinationBuffer, uint destinationOffset, uint bytesToCopy)
    {
        var inputStream = sourceBuffer.AsStream();
        inputStream.Position = sourceOffset;

        var outputStream = destinationBuffer.AsStream();
        outputStream.Position = destinationOffset;

        int blockSize = Math.Min((int)bytesToCopy, 65536);
        byte[] buffer = new byte[blockSize];

        while (bytesToCopy > 0)
        {
            var bytesRead = inputStream.Read(buffer, 0, blockSize);
            outputStream.Write(buffer, 0, bytesRead);

            bytesToCopy -= (uint) bytesRead;
        }
    }

    /// <summary>
    /// This method will always make a request to the remote host for the specific part requested
    /// </summary>
    /// <param name="position">The position to read from</param>
    /// <param name="count">Maximum bytes to read (optional)</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    private async Task<ContentInfo> ExecuteReadAsync(ulong position, uint? count, CancellationToken cancellationToken)
    {
        Debug.Assert(_httpClient != null);

        using var request = CreateHttpRequestMessage(_requestUri, position, count);
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendRequestAsync(request, HttpCompletionOption.ResponseHeadersRead)
                .AsTask(cancellationToken, this).ConfigureAwait(false);
            HttpGetRequestCount++;
        }
        catch (Exception ex)
        {
            throw NetworkException.CreateFromWinRT(_requestUri, request.TransportInformation.ServerCertificate, ex);
        }

        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            await response.AssertSuccessAsync();

            var info = ParseResponse(response);
            AssertCorrectPosition(info.Position, position);

            // Ensure we have a content type
            ContentType ??= response.Content.Headers.ContentType?.MediaType;
            if (_size == 0)
                _size = info.TotalLength;

            // Start reporting progress
            _progressOffset = info.Position;
            _progress?.Report(_progressOffset);

            // Read content as a new buffer. This will report progress
            // as we go..
            info.Buffer = await response.Content.ReadAsBufferAsync()
                .AsTask(cancellationToken, this).ConfigureAwait(false);

            HttpBytesRead += info.Buffer.Length;

            // Report last position
            _progress?.Report(_progressOffset + info.Buffer.Length);

            Debug.Assert(info.Buffer.Length == info.ExpectedContentLength, "Expected buffer to contain all data");

            return info;
        }
        finally
        {
            response.Dispose();
        }
    }

    public void Dispose()
    {
        CanRead = false;
        _cachedResponse = null;
        _isDisposed = true;
    }

    public IAsyncOperation<bool> FlushAsync()
    {
        throw new NotSupportedException();
    }

    public IOutputStream GetOutputStreamAt(ulong position)
    {
        throw new NotSupportedException();
    }

    public IAsyncOperationWithProgress<uint, uint> WriteAsync(IBuffer buffer)
    {
        throw new NotSupportedException("Stream is not writeable");
    }

    private static HttpRequestMessage CreateHttpRequestMessage(Uri uri, ulong position, uint? bytes)
    {
        // Add a header to indicate what part of the media file we request
        string requestedRange = bytes > 0
            ? $"bytes={position}-{position + bytes - 1}"
            : $"bytes={position}-"; // No end position

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = uri,
        };
        request.Headers.Add("Range", requestedRange);
        return request;
    }

    private static ContentInfo ParseResponse(HttpResponseMessage response)
    {
        // We must determine what the content actually contains
        var contentRange = response.Content.Headers.ContentRange;

        if (contentRange != null)
        {
            // Get from range header
            if (!contentRange.Unit.Equals("bytes"))
                throw new NotSupportedException($"The response Content-range indicate units other than bytes: {contentRange.Unit ?? "<null>"}");

            var start = contentRange.FirstBytePosition ?? 0;
            ulong rangeLength;
            if (contentRange.LastBytePosition != null)
                rangeLength = contentRange.LastBytePosition.Value - start + 1;
            else
                rangeLength = contentRange.Length ?? throw new NotSupportedException("Content range does not indicate the full length");

            return new ContentInfo
            {
                Position = contentRange.FirstBytePosition ?? 0,
                ExpectedContentLength = rangeLength,
                TotalLength = contentRange.Length ?? throw new NotSupportedException("The server does not indicate the content length")
            };
        }
        else
        {
            ulong length;
            if (response.Content.Headers.ContentLength != null)
                length = response.Content.Headers.ContentLength.Value;
            else if (response.Headers.TryGetValue("vidiview-content-length", out var contentLengthString)
                && (ulong.TryParse(contentLengthString, out length)))
            {
            }
            else
                throw new NotSupportedException("The server does not indicate the content length");

            return new ContentInfo
            {
                Position = 0,
                ExpectedContentLength = length,
                TotalLength = length
            };
        }
    }

    private static void AssertCorrectPosition(ulong responsePosition, ulong requestedPosition)
    {
        if (responsePosition != requestedPosition)
        {
            throw new NotSupportedException(
                $"The host returned content that is not at the requested position: {responsePosition} != {requestedPosition}");
        }
    }

    void IProgress<HttpProgress>.Report(HttpProgress value)
    {
        //        Debug.WriteLine($"Receiving {value.BytesReceived} / {value.TotalBytesToReceive} ({value.Stage})");
    }

    void IProgress<ulong>.Report(ulong value)
    {
        _progress?.Report(value + _progressOffset);
    }

    private class ContentInfo
    {
        public ulong Position;
        public ulong TotalLength;
        public ulong ExpectedContentLength;
        public IBuffer? Buffer;

        /// <summary>
        /// Returns true if the supplied position is within the content buffer
        /// </summary>
        /// <param name="position"></param>
        /// <param name="offset">The offset from the start of the buffer</param>
        /// <returns></returns>
        public bool PositionWithinBuffer(ulong position, out uint offset)
        {
            if (Buffer != null)
            {
                long o = (long)position - (long)Position;

                offset = (uint)o;
                return o >= 0 && o < Buffer.Length;
            }
            offset = 0;
            return false;
        }
    }
}
#endif