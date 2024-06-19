namespace VidiView.Api.WSMessaging;

/// <summary>
/// Sent when beginning review of study
/// </summary>
public class StudyBeginReviewMessage : IWSMessage
{
    public string MessageType { get; init; }
    public string MessageId { get; init; }

    /// <summary>
    /// The study that is under review by the user
    /// </summary>
    public Guid StudyId { get; set; }

}
