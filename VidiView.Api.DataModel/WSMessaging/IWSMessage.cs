namespace VidiView.Api.WSMessaging;

/// <summary>
/// Messages to be sent over the WebSocket to a VidiView Server must implement this interface
/// </summary>
public interface IWSMessage
{
    /// <summary>
    /// The name of the type, including namespace but not assembly.
    /// Generic type parameters must be named likewise
    /// </summary>
    string MessageType { get; init; }

    /// <summary>
    /// Required message id. Must be unique
    /// </summary>
    string MessageId { get; init; }
}
