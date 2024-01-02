namespace VidiView.Api.DataModel;

public record BookmarkedStudyCollection
{
    /// <summary>
    /// Number of items in this collection
    /// </summary>
    public int Count { get; init; }

    public DateTime FromDate { get; init; }
    public DateTime ToDateExclusive { get; init; }
    public Guid? DepartmentId { get; init; }
    public int MaximumHits { get; init; }

    /// <summary>
    /// The items
    /// </summary>
    public BookmarkedStudy[] Items => Embedded.Studies;

    /// <summary>
    /// Any HAL Rest links associated with this collection
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }

    [JsonPropertyName("_embedded")]
    public EmbeddedArray Embedded { get; init; }

    public record EmbeddedArray
    {
        public BookmarkedStudy[] Studies { get; init; }
    }
}
