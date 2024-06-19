namespace VidiView.Api.WSMessaging;

public class EchoReplyMessage : IWSReply
{
    public string MessageType { get; init; }
    public string MessageId { get; init; }
    public string InReplyTo { get; init; }
    public string Text { get; set; }
}