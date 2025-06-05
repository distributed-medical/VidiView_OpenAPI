using VidiView.Api.DataModel;

namespace VidiView.Api.WSMessaging;

/// <summary>
/// This is sent to all clients having a specific study open, when a queued item is updated
/// </summary>
[ExcludeFromCodeCoverage]
public record QueuedItemUpdatedMessage : IWSMessage
{
    public QueuedItemUpdatedMessage()
    {
        // Maybe it seems odd to use the ToString() here instead
        // of FullName, but the ToString will not return assembly qualified name
        // for generic type parameters
        MessageType = this.GetType().ToString();
        MessageId = Guid.NewGuid().ToString("N");
    }
    public string MessageType { get; init; }
    public string MessageId { get; init; }

    public Guid StudyId { get; init; }

    /// <summary>
    /// The updated status
    /// </summary>
    public QueueItemStatus Status { get; init; }

}

