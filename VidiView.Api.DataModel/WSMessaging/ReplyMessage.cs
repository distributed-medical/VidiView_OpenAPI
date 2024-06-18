namespace VidiView.Api.WSMessaging;

public class ReplyMessage : WSMessage
{
    public string InReplyTo { get; init; } = string.Empty;
}