namespace VidiView.Api.Exceptions;

/// <summary>
/// The host responded, but does not appear to be a VidiView Server
/// </summary>
public class E1402_NoVidiViewServerException : E1400_ConnectServerException
{
    public E1402_NoVidiViewServerException(Uri requestedUri, Exception? innerException = null)
        : base(1402, "The host responded, but does not appear to be a VidiView Server", innerException)
    {
        RequestedUri = requestedUri;
    }

    public E1402_NoVidiViewServerException(string message)
        : base(1402, message)
    {
    }
}
