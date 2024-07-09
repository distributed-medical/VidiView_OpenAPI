using VidiView.Api.DataModel;

namespace VidiView.Api.WSMessaging;

/// <summary>
/// This is sent to clients that are reviewing a specific study
/// </summary>
public class MediaFileUpdatedMessage
{
    public string MessageType { get; init; }
    public string MessageId { get; init; }

    /// <summary>
    /// The study this message is intended for
    /// </summary>
    public Guid StudyId { get; set; }

    /// <summary>
    /// The updated media file
    /// </summary>
    public MediaFile MediaFile { get; set; }

    /// <summary>
    /// The user performing the update
    /// </summary>
    public UserAndClient? UpdatedBy { get; init; }
}
