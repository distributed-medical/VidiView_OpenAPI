namespace VidiView.Api.WSMessaging;

/// <summary>
/// Sent when beginning review of study
/// </summary>
public record StudyBeginReviewMessage : IWSMessage
{
    public StudyBeginReviewMessage()
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
    /// The study that is under review by the user
    /// </summary>
    public Guid StudyId { get; set; }

}
