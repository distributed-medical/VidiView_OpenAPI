namespace VidiView.Api.DataModel;

/// <summary>
/// Study assignment
/// </summary>
public record Assignment
{
    /// <summary>
    /// Study id
    /// </summary>
    public Guid StudyId { get; init; }

    /// <summary>
    /// Assigned user
    /// </summary>
    public IdAndName User { get; init; }

    /// <summary>
    /// The date/time the study was assigned to this user
    /// </summary>
    public DateTimeOffset AssignedDate { get; init; }

    /// <summary>
    /// The user who assigned the study to the user (if other than the user himself)
    /// </summary>
    public IdAndName? AssignedBy { get; init; }
}