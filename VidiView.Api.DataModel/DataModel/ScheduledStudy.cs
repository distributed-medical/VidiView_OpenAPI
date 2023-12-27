namespace VidiView.Api.DataModel;

public record ScheduledStudy 
{
    /// <summary>
    /// The unique id
    /// </summary>
    public string StudyInstanceUid { get; init; } = null!;

    /// <summary>
    /// The study accession number
    /// </summary>
    public string? AccessionNumber { get; init; }

    /// <summary>
    /// The associated department
    /// </summary>
    public IdAndName Department { get; init; } = null!;

    /// <summary>
    /// The scheduled start time of this study
    /// </summary>
    public DateTimeOffset ScheduledTime { get; init; }

    /// <summary>
    /// The scheduled patient
    /// </summary>
    public Patient Patient { get; init; } = null!;

    /// <summary>
    /// Comments
    /// </summary>
    public string? Comments { get; init; }

    /// <summary>
    /// Schedule performing physician
    /// </summary>
    public PersonName? PerformingPhysicianName { get; init; }

    /// <summary>
    /// The referring physician
    /// </summary>
    public PersonName? ReferringPhysicianName { get; init; }

    /// <summary>
    /// Any procedure codes
    /// </summary>
    public CodedValue[]? ProcedureCodes { get; init; }

    /// <summary>
    /// The id of an existing study 
    /// </summary>
    /// <remarks>This is set if a study already exists for this scheduled study item</remarks>
    public Guid? StudyId { get; init; }

    /// <summary>
    /// Sop instace to be used for creating this study
    /// </summary>
    public string? SopInstance { get; init; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }

    public override string ToString() => $"{ScheduledTime:HH.mm} {AccessionNumber ?? StudyInstanceUid}";
}
