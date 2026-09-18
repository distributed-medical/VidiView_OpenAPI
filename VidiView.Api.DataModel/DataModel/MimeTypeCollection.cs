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
    /// Get a MIME type by its type string
    /// </summary>
    /// <param name="mimeType"></param>
    /// <exception cref="ArgumentException">Thrown if the mimeType parameter is null or empty.</exception>
    /// <exception cref="NotSupportedException">Thrown if the mimeType is not supported (not present in the collection).</exception>
    public MimeType this[string mimeType]
    {
        get
        {
            if (string.IsNullOrWhiteSpace(mimeType))
                throw new ArgumentException("Mime type cannot be null or empty.", nameof(mimeType));

            var items = Items ?? throw new InvalidOperationException("Mime type collection is not initialized.");
            return items.FirstOrDefault(m => m.Type.Equals(mimeType, StringComparison.OrdinalIgnoreCase))
                   ?? throw new NotSupportedException($"MimeType '{mimeType}' not supported.");
        }
    }

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
