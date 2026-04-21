namespace VidiView.Api.Exceptions;

/// <summary>
/// General exception connecting to a VidiView Server
/// </summary>
public class E1400_ConnectServerException : VidiViewException
{
    public E1400_ConnectServerException(string message, Exception? innerException)
        : base(1400, message, innerException)
    {
    }

    public E1400_ConnectServerException(string message)
        : base(1400, message)
    {
    }

    protected E1400_ConnectServerException(int errorCode, string message)
        : base(errorCode, message)
    {
    }
    protected E1400_ConnectServerException(int errorCode, string message, Exception? innerException)
        : base(errorCode, message, innerException)
    {
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
