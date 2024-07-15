using VidiView.Api.DataModel;

namespace VidiView.Api.WSMessaging;

/// <summary>
/// This is sent when a study is created for a patient
/// </summary>
public class StudyCreatedMessage : IWSMessage
{
    public string MessageType { get; init; }
    public string MessageId { get; init; }

    /// <summary>
    /// The study this message is intended for
    /// </summary>
    public Guid StudyId { get; init; }

    /// <summary>
    /// The patient that the study is created for
    /// </summary>
    public Guid PatientId { get; init; }

    /// <summary>
    /// The user performing the operation
    /// </summary>
    public UserAndClient? Actor { get; init; }
}
