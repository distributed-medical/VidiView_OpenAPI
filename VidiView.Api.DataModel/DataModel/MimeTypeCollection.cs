namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public class MimeTypeCollection
{
    /// <summary>
    /// Number of items in this collection
    /// </summary>
    public int Count { get; init; }

    /// <summary>
    /// The items
    /// </summary>
    public MimeType[] Items => Embedded.MimeTypes;

    /// <summary>
    /// Any HAL Rest links associated with this collection
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }

    [JsonPropertyName("_embedded")]
    public EmbeddedArray Embedded { get; init; }

    public class EmbeddedArray
    {
        public MimeType[] MimeTypes { get; init; }
    }
}
