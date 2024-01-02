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
    public string ContentType { get; init; }
    public IdAndName Department { get; init; } = null!;
    public IdAndName Queue { get; init; } = null!;
    public ExportQueueStatus TransferStatus { get; init; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }
}
