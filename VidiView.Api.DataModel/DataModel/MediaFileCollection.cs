namespace VidiView.Api.DataModel;

public class MediaFileCollection
{
    /// <summary>
    /// Number of items in this collection
    /// </summary>
    public int Count { get; init; }

    public Guid StudyId { get; init; }

    /// <summary>
    /// The items
    /// </summary>
    public MediaFile[] Items => Embedded.MediaFiles;

    /// <summary>
    /// Any HAL Rest links associated with this collection
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }

    [JsonPropertyName("_embedded")]
    public EmbeddedArray Embedded { get; init; }

    public class EmbeddedArray
    {
        // This is the legacy name...
        [JsonPropertyName("images")]
        public MediaFile[] MediaFiles { get; set; }
    }
}
