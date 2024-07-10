using VidiView.Api.DataModel;

namespace VidiView.Api.WSMessaging;

/// <summary>
/// This is sent to clients that are reviewing a specific study
/// </summary>
/// <remarks>This can be sent as both a reply and a status message</remarks>
public class StudyUnderReviewMessage : IWSReply
{
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
    public UserAndClient[] ReviewingUsers { get; init; }
}

