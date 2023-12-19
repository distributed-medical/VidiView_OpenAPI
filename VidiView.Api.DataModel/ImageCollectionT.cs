namespace VidiView.Api.DataModel;

public class ImageCollection<TEntity>
{
    /// <summary>
    /// Number of items in this collection
    /// </summary>
    public int Count { get; init; }

    public Guid StudyId { get; init; }

    /// <summary>
    /// The items
    /// </summary>
    public TEntity[] Items => Embedded.Images;

    /// <summary>
    /// Any HAL Rest links associated with this collection
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection Links { get; init; }

    [JsonPropertyName("_embedded")]
    public EmbeddedArray Embedded { get; init; }

    public class EmbeddedArray
    {
        public TEntity[] Images { get; set; }
    }
}
