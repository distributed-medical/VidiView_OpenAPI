namespace VidiView.Api.WSMessaging;

public class StudyBeginReviewMessage : IWSMessage
{
    public string MessageType { get; init; }
    public string MessageId { get; init; }

    /// <summary>
    /// The study that is under review by the user
    /// </summary>
    public Guid StudyId { get; set; }

}
