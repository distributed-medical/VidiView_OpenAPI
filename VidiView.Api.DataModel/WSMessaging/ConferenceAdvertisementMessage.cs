using VidiView.Api.DataModel;

namespace VidiView.Api.WSMessaging;

/// <summary>
/// This is sent to all clients having a specific study open, when the active
/// conference state for that conference has changed
/// </summary>
public class ConferenceAdvertisementMessage : IWSMessage
{
    public ConferenceAdvertisementMessage()
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
    /// Current active conferences
    /// </summary>
    public ConferenceAdvertisement[] Active { get; init; }

}

