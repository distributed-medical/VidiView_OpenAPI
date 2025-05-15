namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record LoginSessionCollection
{
    /// <summary>
    /// Number of items in this collection
    /// </summary>
    public int Count
    {
        get; init;
    }

    /// <summary>
    /// The items
    /// </summary>
    public LoginSession[] Items => Embedded.Sessions;

    /// <summary>
    /// Any HAL Rest links associated with this collection
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links
    {
        get; init;
    }

    [JsonPropertyName("_embedded")]
    public EmbeddedArray Embedded
    {
        get; init;
    }

    public record EmbeddedArray
    {
        public LoginSession[] Sessions
        {
            get; init;
        }
    }
}
