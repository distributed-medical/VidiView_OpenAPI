namespace VidiView.Api.DataModel;

public class TrustedIssuerCollection
{
    /// <summary>
    /// Number of items in this collection
    /// </summary>
    public int Count { get; init; }

    /// <summary>
    /// The items
    /// </summary>
    public TrustedIssuer[] Items => Embedded.TrustedIssuers;

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }

    [JsonPropertyName("_embedded")]
    public EmbeddedArray Embedded { get; init; }

    public class EmbeddedArray
    {
        public TrustedIssuer[] TrustedIssuers { get; init; }
    }
}
