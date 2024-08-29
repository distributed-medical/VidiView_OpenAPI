using System.Diagnostics;
using System.IO;

namespace VidiView.Api.Helpers;

/// <summary>
/// This stream will serve a range of an underlying stream
/// </summary>
public class RangeStream
    : Stream
{
    readonly Stream _baseStream;

    /// <summary>
    /// Create a stream that will be a view of the underlying stream
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="fromPosition">From position in underlying stream</param>
    /// <param name="maxLength">The max length of this range</param>
    public RangeStream(Stream stream, long fromPosition, long maxLength)
    {
        if (maxLength <= 0)
            throw new ArgumentException("A positive length is required", nameof(maxLength));

        _baseStream = stream ?? throw new ArgumentNullException(nameof(stream));
        Offset = Math.Clamp(fromPosition, 0, stream.Position);
        Length = Math.Min(stream.Length - Offset, maxLength);

        _baseStream.Position = Offset;
    }

    /// <summary>
    /// The offset from start of the underlying stream
    /// </summary>
    public long Offset { get; }

    /// <summary>
    /// If true, the underlying stream will be closed when this stream is closed or disposed
    /// </summary>
    public bool CloseUnderlyingStream { get; set; } = false;

    /// <summary>
    /// Read from stream
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public override int Read(byte[] buffer, int offset, int count)
    {
        // Read from underlying stream
        long bytesAvailable = Math.Max(0, Length - Position);
        count = (int)Math.Min(count, bytesAvailable);
        int bytesRead = _baseStream.Read(buffer, offset, count);
        return bytesRead;
    }

    public override void Close()
    {
        if (CloseUnderlyingStream)
        {
            _baseStream.Close();
        }
    }

    /// <summary>
    /// The length of this range.
    /// </summary>
    public override long Length { get; }

    /// <summary>
    /// The stream position within the range
    /// </summary>
    public override long Position
    {
        get => _baseStream.Position - Offset;
        set => Seek(value, SeekOrigin.Begin);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        // Debug.Assert(false, "This is not the intended use for this stream...");

        long realPos;
        switch (origin)
        {
            case SeekOrigin.Begin:
                realPos = Math.Max(Offset, offset + Offset);
                break;
            case SeekOrigin.Current:
                realPos = Math.Max(Offset, _baseStream.Position + offset);
                break;
            case SeekOrigin.End:
                realPos = Math.Max(Offset, Offset + Length - offset);
                break;
            default:
                throw new NotImplementedException();
        }

        _baseStream.Position = realPos;
        return Position;
    }
    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }
    public override void Flush()
    {
        throw new NotSupportedException();
    }
    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    public override bool CanRead => _baseStream.CanRead;

    public override bool CanSeek => _baseStream.CanSeek;

    public override bool CanWrite => false;
}

