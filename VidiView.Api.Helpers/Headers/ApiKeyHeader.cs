using System;
using System.Buffers.Binary;
using System.Security.Cryptography;

namespace VidiView.Api.Headers;

/// <summary>
/// This is the official implementation of the Api key provider.
/// </summary>
/// <remarks>
/// This should be re-generated regularly since it expires after 24 hours 
/// </remarks>
/// <example>
/// // Default implementation of device thumbprint
/// var sysId = SystemIdentification.GetSystemIdForPublisher();
/// var reader = Windows.Storage.Streams.DataReader.FromBuffer(sysId.Id);
///         if (sysId.Id.Length< 8)
///             throw new NotSupportedException("The length of the system identifier must be at least 8 bytes long");
/// 
/// byte[] deviceId = new byte[sysId.Id.Length];
/// reader.ReadBytes(deviceId);
/// 
/// Guid applicationId = ...
/// byte[] secretKey = ...
///
/// var apiKey = new ApiKeyHeader(applicationId, deviceId, secretKey);
/// </example>
public sealed class ApiKeyHeader
{
    /// <summary>
    /// Default constructor of ApiKeyHeader
    /// </summary>
    /// <param name="appId">The application id</param>
    /// <param name="thumbprint">Thumbprint of device</param>
    /// <param name="secretKey">The secret key assigned to the application</param>
    /// <exception cref="ArgumentException"></exception>
    public ApiKeyHeader(Guid appId, byte[] thumbprint, byte[] secretKey)
        : this(appId, thumbprint, DateTimeOffset.UtcNow.ToUnixTimeSeconds(), secretKey)
    {
    }

    /// <summary>
    /// This constructor is used for unit testing
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="thumbprint"></param>
    /// <param name="instanceTime"></param>
    /// <param name="secretKey"></param>
    /// <exception cref="ArgumentException"></exception>
    public ApiKeyHeader(Guid appId, byte[] thumbprint, long instanceTime, byte[] secretKey)
    {
        if (appId == Guid.Empty)
            throw new ArgumentException("Invalid appId value", nameof(appId));
        if (thumbprint == null || thumbprint.Length < 4 || thumbprint.Length > 255)
            throw new ArgumentException("Invalid thumbprint value");

        AppId = appId;
        Thumbprint = thumbprint;
        InstanceTime = instanceTime;

        KeyHash = CalculateKeyHash(secretKey);
        Value = CreateHeaderValue();
    }

    /// <summary>
    /// The Http header name
    /// </summary>
    public string Name => "X-Api-key";

    /// <summary>
    /// This is the application client type
    /// </summary>
    /// <remarks>This indicates if the client is a VidiView Client, Capture, Controller or other external application</remarks>
    public Guid AppId { get; }

    /// <summary>
    /// The thumbprint of the calling application
    /// </summary>
    /// <remarks>Ideally, this should never change for a specific application installation</remarks>
    public byte[] Thumbprint { get; }

    /// <summary>
    /// The instance time part of the header. 
    /// This is the number of seconds that elapsed since 1970-01-01 (Unix-time)
    /// </summary>
    public long InstanceTime { get; }

    /// <summary>
    /// This is a hash calculated from the rest of the header value together with a secret key
    /// </summary>
    public byte[] KeyHash { get; }

    /// <summary>
    /// The header value as a string
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Get DateTimeOffset
    /// </summary>
    public DateTimeOffset InstanceTimeAsDateTime => DateTimeOffset.FromUnixTimeSeconds(InstanceTime);

    public override string ToString()
    {
        return $"[{Name} {Value}]";
    }

    string CreateHeaderValue()
    {
        var buffer = new byte[512];
        var span = new Span<byte>(buffer);
        var offset = CopyParametersToBuffer(span);

        KeyHash.CopyTo(span[offset..]);
        offset += KeyHash.Length;

        return Convert.ToBase64String(buffer, 0, offset);
    }

    byte[] CalculateKeyHash(byte[] secretKey)
    {
        var buffer = new byte[512];
        var span = new Span<byte>(buffer);
        var offset = CopyParametersToBuffer(span);

        secretKey.CopyTo(span[offset..]);
        offset += secretKey.Length;

        return SHA256.HashData(buffer.AsSpan(0, offset));
    }

    private int CopyParametersToBuffer(Span<byte> span)
    {
        int offset = 0;

        // First 16 bytes is the Application ID Guid
        AppId.ToByteArray().CopyTo(span);
        offset += 16;

        // Length of the thumbprint (maximum 256 bytes in length)
        span[offset] = (byte)Thumbprint.Length;
        offset++;

        // The thumbprint itself
        Thumbprint.CopyTo(span[offset..]);
        offset += Thumbprint.Length;

        // The instance creation time
        BinaryPrimitives.WriteInt64BigEndian(span[offset..], InstanceTime);
        offset += 8;

        return offset;
    }
}
