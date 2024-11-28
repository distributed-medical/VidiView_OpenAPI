using VidiView.Api.DataModel;

namespace VidiView.Api.WSMessaging;

/// <summary>
/// This is sent to all clients having a specific study open, when the active
/// conference state for that conference has changed
/// </summary>
public record ConferenceSourceCameraStateMessage : IWSMessage
{
    public ConferenceSourceCameraStateMessage()
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
    /// The study this message is related to
    /// </summary>
    public Guid StudyId { get; init; }

    /// <summary>
    /// The source that this message is related to
    /// </summary>
    public Guid SourceId { get; init; }

    /// <summary>
    /// The user performing the operation resulting in this message being sent 
    /// </summary>
    public Actor Actor { get; init; }

    /// <summary>
    /// Current magnification (1.2 = 1.2x = 120%)
    /// </summary>
    public double Magnification { get; init; }

    /// <summary>
    /// Current pan position
    /// </summary>
    public double Pan { get; init; }

    /// <summary>
    /// Current zoom position
    /// </summary>
    public double Zoom { get; init; }

    /// <summary>
    /// Current tilt position
    /// </summary>
    public double Tilt { get; init; }


}

