using VidiView.Api.WSMessaging;

namespace VidiView.Api.Helpers;
public class MessageReceivedEventArgs : EventArgs
{
    internal MessageReceivedEventArgs(IWSMessage message)
    {
        Message = message;
    }

    public IWSMessage Message { get; }
}
