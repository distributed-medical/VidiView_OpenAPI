namespace VidiView.Api.WSMessaging;

public class EchoMessage : IWSMessage
{
    public string MessageType { get; init; }
    public string MessageId { get; init; }
    public string Text { get; set; } = string.Empty;
}