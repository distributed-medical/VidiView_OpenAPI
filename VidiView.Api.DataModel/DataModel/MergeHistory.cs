namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record MergeHistory
{
    // public Guid MergeId { get; init; }

    /// <summary>
    /// The record into which the patient was merged
    /// </summary>
    public Guid NewPatientId { get; init; }

    /// <summary>
    /// The deleted record
    /// </summary>
    public Patient Patient { get; init; }

    /// <summary>
    /// Number of studies that was moved 
    /// </summary>
    public int MergedStudyCount { get; init; }

    /// <summary>
    /// Date and time when the merge was performed
    /// </summary>
    public DateTimeOffset MergeDate { get; set; }

    /// <summary>
    /// True if the merge operation was manually conducted
    /// </summary>
    public bool ManualMerge { get; set; }

    /// <summary>
    /// If manually performed, this is the user who performed the action
    /// </summary>
    public IdAndName? MergedBy { get; set; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }
}