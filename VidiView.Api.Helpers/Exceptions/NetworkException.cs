#if WINRT
using System.Runtime.Versioning;
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
        var result = new NetworkException(requestedUri, certificate, $"{status} (HResult=0x{exception.HResult:X8})", status, exception);

        return result;
    }

    private NetworkException(Uri requestedUri, Certificate? certificate, string message, WebErrorStatus status, Exception innerException)
        : base(message, innerException)
    {
        RequestedUri = requestedUri;
        Status = status;
    }

    public Uri RequestedUri { get; }

    public WebErrorStatus Status { get; }
}
#endif
