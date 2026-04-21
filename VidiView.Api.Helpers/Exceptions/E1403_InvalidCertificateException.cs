using System.Security.Cryptography.X509Certificates;

namespace VidiView.Api.Exceptions;

/// <summary>
/// The host did not present a valid certificate
/// </summary>
public class E1403_InvalidCertificateException : E1400_ConnectServerException
{
    public E1403_InvalidCertificateException(string message, Uri requestedUri, Exception? innerException = null)
        : base(1403, message, innerException)
    {
        RequestedUri = requestedUri;
    }

    public E1403_InvalidCertificateException(string message)
        : base(1403, message)
    {
    }

    public X509Certificate2? Certificate { get; set; }
}
