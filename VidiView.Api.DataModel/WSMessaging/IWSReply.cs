namespace VidiView.Api.WSMessaging;

/// <summary>
/// This indicates a reply message, received from the server in response to a message (that has a corresponding reply specified)
/// </summary>
public interface IWSReply : IWSMessage
{
    /// <summary>
    /// MessageId of original request message
    /// </summary>
    string InReplyTo { get; init; }
}