using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;

namespace VidiView.Api.Helpers;

/// <summary>
/// This is a helper stream that supports seeking using range headers
/// </summary>
/// <remarks>
/// Note that the underlying stream is a network stream that
/// does not support streaming and is forward only. Any position 
/// change in this stream will require a new read operation from 
/// the server to take place
/// </remarks>
public class HttpContentStream : Stream
{
    /// <summary>
    /// Create a seekable stream from response 
    /// </summary>
    /// <param name="http"></param>
    /// <param name="response"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotSupportedException"></exception>
    public static async Task<HttpContentStream> CreateFromResponse(HttpClient http, HttpResponseMessage response)
    {
        if (response == null)
            throw new ArgumentNullException(nameof(response));

        var stream = new HttpContentStream(http, response.RequestMessage?.RequestUri
            ?? throw new Exception("Expected the request uri to be set on response"));

        stream.ContentType = response.Content.Headers.ContentType?.MediaType;
        if (response.StatusCode == HttpStatusCode.PartialContent
            && response.Content.Headers.ContentRange?.HasRange == true)
        {
            // Since this is a partial response, it is seekable using ranges
            var range = response.Content.Headers.ContentRange;
            stream._length = range.Length;
            stream._isSeekable = true;
            stream.Position = range.From ?? 0; // Set start position
            await stream.AssignPart(response).ConfigureAwait(false);
        }
        else
        {
            stream._length = response.Content.Headers.ContentLength;
            if (stream._length == null || stream._length <= 0)
            {
                // Check if we have a custom header
                var vcl = (from kvp in response.Headers
                           where kvp.Key.Equals("vidiview-content-length", StringComparison.OrdinalIgnoreCase)
                           select kvp.Value).FirstOrDefault()?.FirstOrDefault();

                if (vcl != null)
                    stream._length = long.Parse(vcl);
            }

            stream._baseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            stream._isSeekable = stream._baseStream.CanSeek || response.Headers.AcceptRanges?.FirstOrDefault() == "bytes";

            if (stream._baseStream is MemoryStream ms
                && ms.Length > 0)
            {
                stream._partLength = ms.Length;
            }

        }

        return stream;
    }

    public const int DefaultRequestBlockSize = 512 * 1024;

    readonly SemaphoreSlim _singleReadLock = new SemaphoreSlim(1);
    readonly CancellationTokenSource _cts = new CancellationTokenSource();

    readonly HttpClient _http;
    readonly Uri _requestUri;
    long? _length;
    bool _isSeekable;
    bool _isDisposed;
    Stream? _baseStream;

    // This is the stream position (relative the start of the file, 
    // not the current part's stream
    long _absolutePosition;
    long _partPosition;
    long _partLength;

    private HttpContentStream(HttpClient http, Uri requestUri)
    {
        _http = http ?? throw new ArgumentNullException(nameof(http));
        _requestUri = requestUri;
    }

    /// <summary>
    /// The indicated content type
    /// </summary>
    public string? ContentType { get; private set; }

    /// <summary>
    /// This is the total length of the stream
    /// </summary>
    /// <exception cref="NotSupportedException">Thrown if the stream length is not known</exception>
    public override long Length => _length ?? throw new NotSupportedException();

    public override bool CanRead => true;

    /// <summary>
    /// Returns treu if this stream is seekable
    /// </summary>
    public override bool CanSeek => _isSeekable;

    public override bool CanWrite => false;

    /// <summary>
    /// The size of each block that is requested from the server
    /// </summary>
    public int RequestBlockSize { get; set; } = DefaultRequestBlockSize;

    public override void Close()
    {
        _isDisposed = true;

        // Cancel any outstanding operation
        _cts.Cancel();

        // Don't dispose a HttpClient response's network stream. It might throw
        // and will be disposed of anyway by the Response object
        if (_baseStream is MemoryStream ms)
            ms.Dispose();
        _baseStream = null;

        base.Close();
    }

    public override long Position
    {
        get => _absolutePosition;
        set
        {
            if (_absolutePosition != value && !_isSeekable)
                throw new NotSupportedException();

            _absolutePosition = value;
        }
    }

    /// <summary>
    /// Read from stream
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException">Thrown if another read operation is currently active on this stream</exception>
    public override int Read(byte[] buffer, int offset, int count)
    {
        if (_isDisposed)
            throw new ObjectDisposedException(nameof(HttpContentStream));
        if (!_singleReadLock.Wait(0))
            throw new InvalidOperationException("A read operation is already in progress on this stream");

        try
        {
            if (!_isSeekable)
            {
                Debug.Assert(_baseStream != null, "Non-seekable stream must have a base stream assigned");
                // Just read from the underlying stream
                return _baseStream.Read(buffer, offset, count);
            }
            else
            {
                if (Position >= Length)
                    return 0; // End of stream

                // We need to make sure we have the requested data in our local buffer
                AssertInRange().GetAwaiter().GetResult(); // Since this call is not async...

                var streamOffset = Position - _partPosition;
                Debug.Assert(_baseStream != null);
                Debug.Assert(streamOffset >= 0 && streamOffset < _partLength);
                Debug.Assert(streamOffset + _partPosition == _absolutePosition);

                _baseStream.Position = streamOffset;
                var bytesRead = _baseStream.Read(buffer, offset, count);
                Debug.Assert(bytesRead > 0, "We should never have to read 0 bytes at the end.. The application logic should have returned already");

                _absolutePosition += bytesRead;

                return bytesRead;
            }
        }
        finally
        {
            _singleReadLock.Release();
        }
    }

    async Task AssertInRange()
    {
        long streamOffset = Position - _partPosition;
        if (streamOffset < 0 || streamOffset >= _partLength)
        {
            // We don't have this data loaded
            var range = new RangeHeaderValue(Position, Position + RequestBlockSize - 1);

            // Create our request
            using (var request = new HttpRequestMessage(HttpMethod.Get, _requestUri))
            {
                request.Headers.Range = range;
                var response = await _http.SendAsync(request, HttpCompletionOption.ResponseContentRead, CancellationToken.None).ConfigureAwait(false)
                    ?? throw new NullReferenceException("Response unexpectedly null after completed send request");
                await response.AssertSuccessAsync();

                await AssignPart(response).ConfigureAwait(false);
            }
        }
    }
    async Task AssignPart(HttpResponseMessage response)
    {
        _baseStream?.Dispose();
        _baseStream = null;
        _partPosition = response.Content.Headers.ContentRange?.From ?? 0;
        _partLength = response.Content.Headers.ContentRange?.To - _partPosition + 1
            ?? Length - _partPosition;

        _baseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        switch (origin)
        {
            case SeekOrigin.Begin:
                Position = offset;
                break;
            case SeekOrigin.Current:
                Position += offset;
                break;
            case SeekOrigin.End:
                Position = Length - offset;
                break;
        }

        return Position;
    }

    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }
    public override void Flush()
    {
        throw new NotSupportedException();
    }
}