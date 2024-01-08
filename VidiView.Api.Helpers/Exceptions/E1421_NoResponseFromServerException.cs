namespace VidiView.Api.Exceptions;

public class E1421_NoResponseFromServerException : VidiViewException
{
    public E1421_NoResponseFromServerException(Uri requestedUri)
        : base("No response from server")
    {
            RequestedUri = requestedUri;
    }

    public E1421_NoResponseFromServerException(string message)
        : base(message)
    {
    }
}
