namespace VidiView.Api.DataModel;

/// <summary>
/// Export status of a media file
/// </summary>
public class ExportMediaFile
{
    public Guid ItemId { get; set; }
    public Guid StudyId { get; set; }
    public Guid ImageId { get; set; }
    public IdAndName AddedBy { get; set; }
    public DateTimeOffset AddedDate { get; set; }
    public string ContentType { get; set; }
    public Department Department { get; set; }
    public IdAndName Queue { get; set; }
    public ExportQueueStatus TransferStatus { get; set; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; set; }
}
