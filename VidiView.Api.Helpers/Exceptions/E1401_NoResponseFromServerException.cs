namespace VidiView.Api.Exceptions;

/// <summary>
/// No response received when calling VidiView Server
/// </summary>
public class E1401_NoResponseFromServerException : E1400_ConnectServerException
{
    public E1401_NoResponseFromServerException(Uri requestedUri, Exception? innerException = null)
        : base("No response from server", innerException)
    {
        RequestedUri = requestedUri;
        ErrorCode = 1401;
    }

    public E1401_NoResponseFromServerException(string message)
        : base(message)
    {
        ErrorCode = 1401;
    }
}
