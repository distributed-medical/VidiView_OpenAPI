#if WINRT

using System.Runtime.Versioning;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using Windows.Security.Cryptography.Certificates;
using Windows.Web;

namespace VidiView.Api.Exceptions;

public class ConnectServerException : Exception
{
    private const int WININET_E_INVALID_CA = unchecked( (int) 0x80072f0d );
    private const int WININET_E_SECURITY_CHANNEL_ERROR = unchecked( (int) 0x80072f7d );

    [SupportedOSPlatform("windows10.0.17763.0")]
    public static E1400_ConnectServerException CreateFromWinRT(Uri requestedUri, Certificate? certificate, Exception exception)
    {
        var status = WebError.GetStatus(exception.HResult);
        
        string? errorMessage = GetFromWebStatus(status)
            ?? GetRestrictedDescription(exception)
            ?? GetFromHR(exception.HResult);

        // Determine exception from web status
        switch (status)
        {
            case Windows.Web.WebErrorStatus.CannotConnect:
                throw new E1401_NoResponseFromServerException(requestedUri, exception)
                {
                    HResult = exception.HResult,
                    Status = status,
                    HostCertificate = certificate
                };

            case Windows.Web.WebErrorStatus.CertificateCommonNameIsIncorrect:
            case Windows.Web.WebErrorStatus.CertificateExpired:
            case Windows.Web.WebErrorStatus.CertificateContainsErrors:
            case Windows.Web.WebErrorStatus.CertificateRevoked:
            case Windows.Web.WebErrorStatus.CertificateIsInvalid:
                throw new E1403_InvalidCertificateException(errorMessage ?? exception.Message, requestedUri, exception)
                {
                    HResult = exception.HResult,
                    Status = status,
                    HostCertificate = certificate
                };
        }

        switch (exception.HResult)
        {
            case WININET_E_INVALID_CA: // This error code is not mapped in Windows?
                throw new E1403_InvalidCertificateException(errorMessage ?? exception.Message, requestedUri, exception)
                {
                    HResult = exception.HResult,
                    Status = status,
                    HostCertificate = certificate
                };

            default:
                throw new E1400_ConnectServerException(errorMessage ?? exception.Message, exception)
                {
                    HResult = exception.HResult,
                    RequestedUri = requestedUri,
                    Status = status,
                    HostCertificate = certificate
                };
        }
    }

    private static string? GetFromHR(int hr)
    {
        switch (hr)
        {
            case WININET_E_INVALID_CA:
                return "The certificate authority is invalid or incorrect";

            case WININET_E_SECURITY_CHANNEL_ERROR:
                return "An error occurred in the secure channel support";

            default:
                var exc = Marshal.GetExceptionForHR(hr);
                if (exc != null)
                {
                    return exc.Message;
                }
                return null;
       }
    }

    private static string? GetRestrictedDescription(Exception exception)
    {
        foreach (DictionaryEntry de in exception.Data)
        {
            if ((string)de.Key == "RestrictedDescription" && !string.IsNullOrEmpty((string?)de.Value))
            {
                return (string)de.Value;
            }
        }
        return null;
    }

    [ExcludeFromCodeCoverage]

    private static string? GetFromWebStatus(WebErrorStatus status)
    {
        switch (status)
        {
            case Windows.Web.WebErrorStatus.Unknown: return null; // "An unknown error has occurred.";
            case Windows.Web.WebErrorStatus.CertificateCommonNameIsIncorrect: return "The SSL certificate common name does not match the host name.";
            case Windows.Web.WebErrorStatus.CertificateExpired: return "The SSL certificate has expired.";
            case Windows.Web.WebErrorStatus.CertificateContainsErrors: return "The SSL certificate contains errors.";
            case Windows.Web.WebErrorStatus.CertificateRevoked: return "The SSL certificate has been revoked.";
            case Windows.Web.WebErrorStatus.CertificateIsInvalid: return "The SSL certificate is invalid.";
            case Windows.Web.WebErrorStatus.ServerUnreachable: return "The server is not responding.";
            case Windows.Web.WebErrorStatus.Timeout: return "The connection has timed out.";
            case Windows.Web.WebErrorStatus.ErrorHttpInvalidServerResponse: return "The server returned an invalid or unrecognized response.";
            case Windows.Web.WebErrorStatus.ConnectionAborted: return "The connection was aborted.";
            case Windows.Web.WebErrorStatus.ConnectionReset: return "The connection was reset.";
            case Windows.Web.WebErrorStatus.Disconnected: return "The connection was ended.";
            case Windows.Web.WebErrorStatus.HttpToHttpsOnRedirection: return "Redirected from a location to a secure location.";
            case Windows.Web.WebErrorStatus.HttpsToHttpOnRedirection: return "Redirected from a secure location to an unsecure location.";
            case Windows.Web.WebErrorStatus.CannotConnect: return "Cannot connect to destination.";
            case Windows.Web.WebErrorStatus.HostNameNotResolved: return "The host name could not be resolved.";
            case Windows.Web.WebErrorStatus.OperationCanceled: return "The operation was canceled.";
            case Windows.Web.WebErrorStatus.RedirectFailed: return "The request redirect failed.";
            case Windows.Web.WebErrorStatus.UnexpectedStatusCode: return "An unexpected status code indicating a failure was received.";
            case Windows.Web.WebErrorStatus.UnexpectedRedirection: return "A request was unexpectedly redirected.";
            case Windows.Web.WebErrorStatus.UnexpectedClientError: return "An unexpected client-side error has occurred.";
            case Windows.Web.WebErrorStatus.UnexpectedServerError: return "An unexpected server-side error has occurred.";
            case Windows.Web.WebErrorStatus.InsufficientRangeSupport: return "The request does not support the range.";
            case Windows.Web.WebErrorStatus.MissingContentLengthSupport: return "The request is mising the file size.";
            case Windows.Web.WebErrorStatus.MultipleChoices: return "The requested URL represents a high level grouping of which lower level selections need to be made.";
            case Windows.Web.WebErrorStatus.MovedPermanently: return "This and all future requests should be directed to the given URI.";
            case Windows.Web.WebErrorStatus.Found: return "The resource was found but is available in a location different from the one included in the request.";
            case Windows.Web.WebErrorStatus.SeeOther: return "The response to the request can be found under another URI using a GET method.";
            case Windows.Web.WebErrorStatus.NotModified: return "Indicates the resource has not been modified since last requested.";
            case Windows.Web.WebErrorStatus.UseProxy: return "The requested resource must be accessed through the proxy given by the Location field.";
            case Windows.Web.WebErrorStatus.TemporaryRedirect: return "The requested resource resides temporarily under a different URI.";
            case Windows.Web.WebErrorStatus.BadRequest: return "The request cannot be fulfilled due to bad syntax.";
            case Windows.Web.WebErrorStatus.Unauthorized: return "Authentication has failed or credentials have not yet been provided.";
            case Windows.Web.WebErrorStatus.PaymentRequired: return "Payment required.";
            case Windows.Web.WebErrorStatus.Forbidden: return "The server has refused the request.";
            case Windows.Web.WebErrorStatus.NotFound: return "The requested resource could not be found but may be available again in the future.";
            case Windows.Web.WebErrorStatus.MethodNotAllowed: return "A request was made of a resource using a request method not supported by that resource.";
            case Windows.Web.WebErrorStatus.NotAcceptable: return "The requested resource is only capable of generating content not acceptable according to the Accept headers sent in the request.";
            case Windows.Web.WebErrorStatus.ProxyAuthenticationRequired: return "The client must first authenticate itself with the proxy.";
            case Windows.Web.WebErrorStatus.RequestTimeout: return "The server timed out waiting for the request.";
            case Windows.Web.WebErrorStatus.Conflict: return "Indicates that the request could not be processed because of conflict in the request.";
            case Windows.Web.WebErrorStatus.Gone: return "Indicates that the resource requested is no longer available and will not be available again.";
            case Windows.Web.WebErrorStatus.LengthRequired: return "The request did not specify the length of its content, which is required by the requested resource.";
            case Windows.Web.WebErrorStatus.PreconditionFailed: return "The server does not meet one of the preconditions that the requester put on the request.";
            case Windows.Web.WebErrorStatus.RequestEntityTooLarge: return "The request is larger than the server is willing or able to process.";
            case Windows.Web.WebErrorStatus.RequestUriTooLong: return "Provided URI length exceeds the maximum length the server can process.";
            case Windows.Web.WebErrorStatus.UnsupportedMediaType: return "The request entity has a media type which the server or resource does not support.";
            case Windows.Web.WebErrorStatus.RequestedRangeNotSatisfiable: return "The client has asked for a portion of the file, but the server cannot supply that portion.";
            case Windows.Web.WebErrorStatus.ExpectationFailed: return "The server cannot meet the requirements of the Expect request-header field.";
            case Windows.Web.WebErrorStatus.InternalServerError: return "A generic error message, given when no more specific message is suitable.";
            case Windows.Web.WebErrorStatus.NotImplemented: return "The server either does not recognize the request method, or it lacks the ability to fulfill the request.";
            case Windows.Web.WebErrorStatus.BadGateway: return "The server was acting as a gateway or proxy and received an invalid response from the upstream server.";
            case Windows.Web.WebErrorStatus.ServiceUnavailable: return "The server is currently unavailable.";
            case Windows.Web.WebErrorStatus.GatewayTimeout: return "The server was acting as a gateway or proxy and did not receive a timely response from the upstream server.";
            case Windows.Web.WebErrorStatus.HttpVersionNotSupported: return "The server does not support the HTTP protocol version used in the request.";
            default:
                return status.ToString();
        }
    }
}
#endif
