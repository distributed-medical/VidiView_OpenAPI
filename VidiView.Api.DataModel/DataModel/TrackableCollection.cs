namespace VidiView.Api.DataModel;

public record TrackableCollection
{
    /// <summary>
    /// Number of items in this collection
    /// </summary>
    public int Count { get; init; }

    public Guid PatientId { get; init; }
    public int MaximumHits { get; init; }

    /// <summary>
    /// The items
    /// </summary>
    public Trackable[] Items => Embedded.Trackables;

    /// <summary>
    /// Any HAL Rest links associated with this collection
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }

    [JsonPropertyName("_embedded")]
    public EmbeddedArray Embedded { get; init; }

    public record EmbeddedArray
    {
        public Trackable[] Trackables { get; init; }
    }
}
