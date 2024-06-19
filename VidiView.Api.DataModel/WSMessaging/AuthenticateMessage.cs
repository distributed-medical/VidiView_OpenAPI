namespace VidiView.Api.WSMessaging;

/// <summary>
/// Authentication message. Must be the first message sent after connecting a WebSocket
/// </summary>
public class AuthenticateMessage : IWSMessage
{
    public string MessageType { get; init; }
    public string MessageId { get; init; }

    /// <summary>
    /// The value of the ApiKey header
    /// </summary>
    public string ApiKey { get; set; }

    /// <summary>
    /// The value of the authorization bearer token
    /// </summary>
    public string Authorization { get; set; }
}