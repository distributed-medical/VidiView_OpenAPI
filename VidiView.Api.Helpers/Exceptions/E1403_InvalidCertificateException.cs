namespace VidiView.Api.Exceptions;

/// <summary>
/// The host did not present a valid certificate
/// </summary>
public class E1403_InvalidCertificateException : E1400_ConnectServerException
{
    public E1403_InvalidCertificateException(string message, Uri requestedUri, Exception? innerException = null)
        : base(message, innerException)
    {
        RequestedUri = requestedUri;
        ErrorCode = 1403;
    }

    public E1403_InvalidCertificateException(string message)
        : base(message)
    {
        ErrorCode = 1402;
    }
}
