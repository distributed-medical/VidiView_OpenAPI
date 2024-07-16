using VidiView.Api.DataModel;

namespace VidiView.Api.WSMessaging;

/// <summary>
/// This is sent to clients that are reviewing a specific study
/// </summary>
public class AnnotationDeletedMessage : IWSMessage
{
    public string MessageType { get; init; }
    public string MessageId { get; init; }

    /// <summary>
    /// The study this message is intended for
    /// </summary>
    public Guid StudyId { get; init; }

    /// <summary>
    /// The Id of the deleted media file
    /// </summary>
    public Guid ImageId { get; init; }

    /// <summary>
    /// The deleted annotation
    /// </summary>
    public Guid AnnotationId { get; init; }

    /// <summary>
    /// The user performing the operation
    /// </summary>
    public UserAndClient? Actor { get; init; }
}
