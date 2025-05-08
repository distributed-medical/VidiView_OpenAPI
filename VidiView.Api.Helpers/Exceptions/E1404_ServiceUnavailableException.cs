namespace VidiView.Api.Exceptions;

public class E1404_ServiceUnavailableException : E1400_ConnectServerException
{
    public E1404_ServiceUnavailableException(Uri requestedUri, Exception? innerException, string message = "Service unavailable")
        : base(message, innerException)
    {
        ErrorCode = 1404;
        RequestedUri = requestedUri;
    }
}
