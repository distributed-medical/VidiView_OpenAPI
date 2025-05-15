namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record AuditEvent
{
    /// <summary>
    /// The time this events was logged
    /// </summary>
    public DateTimeOffset Time { get; init; }

    /// <summary>
    /// Event id
    /// </summary>
    /// <remarks>Either this, or Events array will be set</remarks>
    public int? Event { get; init; }

    /// <summary>
    /// Event id's
    /// </summary>
    /// <remarks>Either this, or Event property will be set</remarks>
    public int[]? Events { get; init; }

    /// <summary>
    /// The affected entity, such as a study or media file
    /// </summary>
    /// <remarks>The type of entity can be deducted from the event id</remarks>
    public Guid? EntityId { get; init; }

    /// <summary>
    /// Id of patient this event relates to
    /// </summary>
    public PatientId? PatientId { get; init; }

    /// <summary>
    /// Name of patient this event relates to
    /// </summary>
    public PersonName? PatientName { get; init; }

    /// <summary>
    /// The acting user causing the event to be logged
    /// </summary>
    public IdAndName User { get; init; }

    /// <summary>
    /// Additional event data
    /// </summary>
    public string? Data { get; init; }

    /// <summary>
    /// The study ID
    /// </summary>
    /// <remarks>Only returned for study interactions requests</remarks>
    public Guid? StudyId { get; init; }

    /// <summary>
    /// The study accession number
    /// </summary>
    /// <remarks>Only returned for study interactions requests</remarks>
    public string? AccessionNumber { get; init; }

    /// <summary>
    /// The department
    /// </summary>
    /// <remarks>Only returned for study interactions requests</remarks>
    public IdAndName? Department { get; init; }

}
