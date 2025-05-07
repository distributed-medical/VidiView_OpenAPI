namespace VidiView.Api.Exceptions;

public class E1421_NoResponseFromServerException : VidiViewException
{
    public E1421_NoResponseFromServerException(Uri requestedUri, Exception? innerException = null)
        : base("No response from server", innerException)
    {
        RequestedUri = requestedUri;
        ErrorCode = 1421;
    }

    public E1421_NoResponseFromServerException(string message)
        : base(message)
    {
        ErrorCode = 1421;
    }
}
