using VidiView.Api.DataModel;

namespace VidiView.Api.WSMessaging;

/// <summary>
/// Received as a response to a successful authentication
/// </summary>
public class AuthenticateReplyMessage : IWSReply
{
    public string MessageType { get; init; }
    public string MessageId { get; init; }
    public string InReplyTo { get; init; }
    /// <summary>
    /// The authenticated user
    /// </summary>
    public User User { get; init; }
}