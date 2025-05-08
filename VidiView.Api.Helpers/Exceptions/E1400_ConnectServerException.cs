namespace VidiView.Api.Exceptions;

/// <summary>
/// General exception connecting to a VidiView Server
/// </summary>
public class E1400_ConnectServerException : VidiViewException
{
    public E1400_ConnectServerException(string message, Exception? innerException)
        : base(message, innerException)
    {
        ErrorCode = 1400;
    }

    public E1400_ConnectServerException(string message)
        : base(message)
    {
        ErrorCode = 1400;
    }

#if WINRT
    /// <summary>
    /// The certificate presented by the host
    /// </summary>
    public Windows.Security.Cryptography.Certificates.Certificate? HostCertificate { get; internal set; }

    /// <summary>
    /// Status code
    /// </summary>
    public Windows.Web.WebErrorStatus Status { get; internal set; }
#endif
}
