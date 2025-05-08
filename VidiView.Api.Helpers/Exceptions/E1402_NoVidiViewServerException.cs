namespace VidiView.Api.Exceptions;

/// <summary>
/// The host responded, but does not appear to be a VidiView Server
/// </summary>
public class E1402_NoVidiViewServerException : E1400_ConnectServerException
{
    public E1402_NoVidiViewServerException(Uri requestedUri, Exception? innerException = null)
        : base("The host responded, but does not appear to be a VidiView Server", innerException)
    {
        RequestedUri = requestedUri;
        ErrorCode = 1402;
    }

    public E1402_NoVidiViewServerException(string message)
        : base(message)
    {
        ErrorCode = 1402;
    }
}
