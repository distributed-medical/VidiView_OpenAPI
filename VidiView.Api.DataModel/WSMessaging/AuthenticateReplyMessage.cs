using VidiView.Api.DataModel;

namespace VidiView.Api.WSMessaging;

public class AuthenticateReplyMessage : IWSReply
{
    public string MessageType { get; init; }
    public string MessageId { get; init; }
    public string InReplyTo { get; init; }
    public User User { get; init; }
}