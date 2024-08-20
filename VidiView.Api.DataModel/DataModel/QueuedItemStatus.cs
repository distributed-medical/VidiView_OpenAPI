namespace VidiView.Api.DataModel;

/// <summary>
/// Export status of a media file
/// </summary>
public record QueuedItemStatus
{
    /// <summary>
    /// Queued item id
    /// </summary>
    public Guid Id { get; init; }
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
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }
}
