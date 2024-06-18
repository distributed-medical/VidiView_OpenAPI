using VidiView.Api.DataModel;

namespace VidiView.Api.WSMessaging;

public class AuthenticateReplyMessage : ReplyMessage
{
    public User User { get; init; }
}