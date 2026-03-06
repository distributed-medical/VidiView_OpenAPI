using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidiView.Api.Headers;

public class ReprDigestHeader
{
    public const string HeaderName = "Repr-Digest";

    public static ReprDigestHeader CreateSha256(byte[] checksum)
    {
        ArgumentNullException.ThrowIfNull(checksum, nameof(checksum));
        if (checksum.Length == 0)
        {
            throw new ArgumentNullException("Zero length checksum not allowed");
        }

        return CreateSha256(Convert.ToBase64String(checksum));
    }

    public static ReprDigestHeader CreateSha256(string base64Checksum)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(base64Checksum, nameof(base64Checksum));

        return new ReprDigestHeader("sha-256", ":" + base64Checksum + ":");
    }

    public ReprDigestHeader()
    {
    }

    public ReprDigestHeader(string algorithm, string value)
    {
        Algorithm = algorithm;
        Value = value;
    }

    /// <summary>
    /// The Http header name
    /// </summary>
    public string Name => HeaderName;

    public string Algorithm { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{Algorithm}={Value}";
    }

}
