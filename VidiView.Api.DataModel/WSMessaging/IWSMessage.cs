namespace VidiView.Api.WSMessaging;
public interface IWSMessage
{
    string MessageType { get; init; }
    string MessageId { get; init; }
}
