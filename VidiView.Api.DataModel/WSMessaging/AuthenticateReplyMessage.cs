using VidiView.Api.DataModel;

namespace VidiView.Api.WSMessaging;

/// <summary>
/// Received as a response to a successful authentication
/// </summary>
[ExcludeFromCodeCoverage]
public record AuthenticateReplyMessage : IWSReply
{
    public AuthenticateReplyMessage()
    {
        // Maybe it seems odd to use the ToString() here instead
        // of FullName, but the ToString will not return assembly qualified name
        // for generic type parameters
        MessageType = this.GetType().ToString();
        MessageId = Guid.NewGuid().ToString("N");
    }

    public string MessageType { get; init; }
    public string MessageId { get; init; }
    public string InReplyTo { get; init; }
    /// <summary>
    /// The authenticated user
    /// </summary>
    public User User { get; init; }
}