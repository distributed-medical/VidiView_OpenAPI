namespace VidiView.Api.DataModel;

/// <summary>
/// Export status of a media file
/// </summary>
public record ExportMediaFile
{
    public Guid ItemId { get; init; }
    public Guid StudyId { get; init; }
    public Guid ImageId { get; init; }
    public IdAndName AddedBy { get; init; } = null!;
    public DateTimeOffset AddedDate { get; init; }
    public IdAndName Queue { get; init; } = null!;

    /// <summary>
    /// Export status
    /// </summary>
    public ExportQueueStatus Status { get; init; }
    public string? StatusDescription { get; init; }

    public IdAndName? Department { get; init; }

    /// <summary>
    /// Media file acquisition date
    /// </summary>
    public DateTimeOffset? AcquisitionDate { get; init; }
    /// <summary>
    /// Media file type
    /// </summary>
    public string? ContentType { get; init; }
    /// <summary>
    /// Media file size
    /// </summary>
    public long? FileSize { get; init; }
    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }
}
