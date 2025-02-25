using VidiView.Api.DataModel;

namespace VidiView.Api.WSMessaging;

/// <summary>
/// This is sent to all conference participants, when conference state is updated.
/// </summary>
public record ConferenceSourceStateMessage : IWSMessage
{
    public ConferenceSourceStateMessage()
    {
        // Maybe it seems odd to use the ToString() here instead
        // of FullName, but the ToString will not return assembly qualified name
        // for generic type parameters
        MessageType = this.GetType().ToString();
        MessageId = Guid.NewGuid().ToString("N");
    }

    public string MessageType { get; init; }
    public string MessageId { get; init; }

    /// <summary>
    /// The conference id
    /// </summary>
    public Guid ConferenceId { get; init; }

    /// <summary>
    /// The source Id
    /// </summary>
    public Guid SourceId { get; init; }

    /// <summary>
    /// Flags for this source
    /// </summary>
    public ConferenceSourceFlags Flags { get; init; }

    /// <summary>
    /// Time when the current recording started
    /// </summary>
    public DateTimeOffset? RecordingStarted { get; init; }
}
