namespace VidiView.Api.DataModel.Exceptions;

public class E1404_ServiceUnavailableException : VidiViewException
{
    public E1404_ServiceUnavailableException(Uri calledUri, Exception? innerException, string message = "Service unavailable")
        : base(message, innerException)
    {
        ErrorCode = 1404;
    }
}
