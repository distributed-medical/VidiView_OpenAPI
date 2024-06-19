namespace VidiView.Api.WSMessaging;

public interface IWSReply : IWSMessage
{
    string InReplyTo { get; init; }
}