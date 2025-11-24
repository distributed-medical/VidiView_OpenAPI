using VidiView.Api.DataModel;

namespace VidiView.Api.WSMessaging;

/// <summary>
/// This is sent to all clients having a specific study open, when a 
/// file is being restored from an external archive
/// </summary>
[ExcludeFromCodeCoverage]
public record RestoreFileStatusMessage : IWSMessage
{
    public RestoreFileStatusMessage()
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
    /// Study this conference relates to
    /// </summary>
    public Guid StudyId { get; init; }

    /// <summary>
    /// Current restore status
    /// </summary>
    public RestoreItemRequest Status { get; init; }

}

