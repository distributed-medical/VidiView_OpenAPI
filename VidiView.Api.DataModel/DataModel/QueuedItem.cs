namespace VidiView.Api.DataModel;

/// <summary>
/// Export status of a media file
/// </summary>
public record QueuedItem
{
    /// <summary>
    /// Queued item id
    /// </summary>
    public Guid Id { get; init; }
    /// <summary>
    /// Study id
    /// </summary>
    public Guid StudyId { get; init; }
    /// <summary>
    /// Media file id
    /// </summary>
    public Guid ImageId { get; init; }
    /// <summary>
    /// Id and name of export queue
    /// </summary>
    public IdAndName Queue { get; init; } = null!;
    /// <summary>
    /// Added to export queue by user
    /// </summary>
    public IdAndName AddedBy { get; init; } = null!;
    /// <summary>
    /// Added to export queue time
    /// </summary>
    public DateTimeOffset AddedDate { get; init; }
    /// <summary>
    /// Export status
    /// </summary>
    public ExportStatus Status { get; init; }

    /// <summary>
    /// More detailed status
    /// </summary>
    public string? StatusDescription { get; init; }

    /// <summary>
    /// Current progress (transfer/conversion)
    /// Percentage value (0 <= x <= 1)
    /// </summary>
    public float? Progress { get; init; }

    /// <summary>
    /// The department the study belongs to
    /// </summary>
    public IdAndName Department { get; init; }

    /// <summary>
    /// The study accession number, if any
    /// </summary>
    public string? AccessionNumber { get; init; }

    /// <summary>
    /// Media file acquisition date
    /// </summary>
    public DateTimeOffset AcquisitionDate { get; init; }

    /// <summary>
    /// The patient this item is associated with
    /// </summary>
    public Patient? Patient { get; init; }

    /// <summary>
    /// Media file type
    /// </summary>
    public string ContentType { get; init; }

    /// <summary>
    /// Media file size
    /// </summary>
    public long FileSize { get; init; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }
}
