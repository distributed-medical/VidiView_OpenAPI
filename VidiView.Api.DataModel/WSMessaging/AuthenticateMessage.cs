namespace VidiView.Api.WSMessaging;

public class AuthenticateMessage : IWSMessage
{
    public string MessageType { get; init; }
    public string MessageId { get; init; }
    public string ApiKey { get; set; }
    public string Authorization { get; set; }
}