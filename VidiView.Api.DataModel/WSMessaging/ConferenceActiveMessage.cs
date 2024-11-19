using VidiView.Api.DataModel;

namespace VidiView.Api.WSMessaging;

/// <summary>
/// This is sent to all clients having a specific study open, when the active
/// conference state for that conference has changed
/// </summary>
public class ConferenceActiveMessage : IWSMessage
{
    public ConferenceActiveMessage()
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
    /// Patient this conference relates to. If null, it is an unidentified study
    /// </summary>
    public Guid? PatientId { get; init; }

    /// <summary>
    /// Study this conference relates to
    /// </summary>
    public Guid StudyId { get; init; }

    /// <summary>
    /// The active conference (if null, conference is disabled)
    /// </summary>
    public Conference? Active { get; init; }

}

