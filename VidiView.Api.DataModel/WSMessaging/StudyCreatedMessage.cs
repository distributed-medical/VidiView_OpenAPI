using VidiView.Api.DataModel;

namespace VidiView.Api.WSMessaging;

/// <summary>
/// This is sent when a study is created for a patient
/// </summary>
public class StudyCreatedMessage : IWSActorMessage
{
    public StudyCreatedMessage()
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
    /// The user performing the operation
    /// </summary>
    public UserAndClient Actor { get; init; }

    /// <summary>
    /// The department in which the study is created
    /// </summary>
    public Guid DepartmentId { get; init; }

    /// <summary>
    /// The study this message is intended for
    /// </summary>
    public Guid StudyId { get; init; }

    /// <summary>
    /// The patient that the study is created for
    /// </summary>
    public Guid PatientId { get; init; }


}
