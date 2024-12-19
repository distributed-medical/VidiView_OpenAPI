#if WINRT
using System.Runtime.Versioning;
using System.Runtime.InteropServices;
using Windows.Security.Cryptography.Certificates;
using Windows.Web;

namespace VidiView.Api.Exceptions;

public class NetworkException : Exception
{
    [SupportedOSPlatform("windows10.0.17763.0")]
    public static Exception CreateFromWinRT(Uri requestedUri, Certificate? certificate, Exception exception)
    {
        if (exception is OperationCanceledException)
            return exception;

        var status = WebError.GetStatus(exception.HResult);
        string statusDescription;
        if (status != WebErrorStatus.Unknown)
        {
            statusDescription = $"{status} (HResult=0x{exception.HResult:X8})";
        }
        else
        {
            if (!string.IsNullOrEmpty(exception.Message))
                statusDescription = exception.Message;
            else 
                statusDescription = Marshal.GetExceptionForHR(exception.HResult)?.Message ?? $"(HResult=0x{exception.HResult:X8})";
        }

        var result = new NetworkException(requestedUri, certificate, statusDescription, status, exception);

        return result;
    }

    private NetworkException(Uri requestedUri, Certificate? certificate, string message, WebErrorStatus status, Exception innerException)
        : base(message, innerException)
    {
        RequestedUri = requestedUri;
        HostCertificate = certificate;
        Status = status;
    }

    /// <summary>
    /// The certificate presented by the host
    /// </summary>
    public Certificate? HostCertificate { get; }

    /// <summary>
    /// The URI that was requested
    /// </summary>
    public Uri RequestedUri { get; }

    /// <summary>
    /// Status code
    /// </summary>
    public WebErrorStatus Status { get; }
}
#endif
