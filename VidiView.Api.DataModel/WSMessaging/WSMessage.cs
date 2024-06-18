namespace VidiView.Api.WSMessaging;

public abstract class WSMessage
{
    public WSMessage()
    {
        MessageType = this.GetType().Name;
        MessageId = Guid.NewGuid().ToString("N");
    }

    public string MessageType { get; init; }
    public string MessageId { get; init; }
}