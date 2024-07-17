using VidiView.Api.DataModel;

namespace VidiView.Api.WSMessaging;

/// <summary>
/// This is sent to clients that are reviewing a specific study
/// </summary>
/// <remarks>This can be sent as both a reply and a status message</remarks>
public class StudyUnderReviewMessage : IWSReply
{
    public StudyUnderReviewMessage()
    {
        // Maybe it seems odd to use the ToString() here instead
        // of FullName, but the ToString will not return assembly qualified name
        // for generic type parameters
        MessageType = this.GetType().ToString();
        MessageId = Guid.NewGuid().ToString("N");
    }
    public string MessageType { get; init; }
    public string MessageId { get; init; }
    public string InReplyTo { get; init; }

    /// <summary>
    /// The study this message is intended for
    /// </summary>
    public Guid StudyId { get; init; }

    /// <summary>
    /// A list of users currently having the study under review
    /// along with the device name from where the review is performed
    /// </summary>
    public Actor[] ReviewingUsers { get; init; }
}

