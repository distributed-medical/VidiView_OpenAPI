namespace VidiView.Api.WSMessaging;

public record StudyEndReviewMessage : IWSMessage
{
    public StudyEndReviewMessage()
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
    /// The study that is no longer under review by the user
    /// </summary>
    public Guid StudyId { get; set; }

}
