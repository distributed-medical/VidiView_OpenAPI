namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public class ClientDeviceCollection
{
    /// <summary>
    /// Number of items in this collection
    /// </summary>
    public int Count { get; init; }

    /// <summary>
    /// The items
    /// </summary>
    public ClientDevice[] Items => Embedded.Devices;

    /// <summary>
    /// Any HAL Rest links associated with this collection
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }

    [JsonPropertyName("_embedded")]
    public EmbeddedArray Embedded { get; init; }

    public class EmbeddedArray
    {
        public ClientDevice[] Devices { get; init; }
    }
}
