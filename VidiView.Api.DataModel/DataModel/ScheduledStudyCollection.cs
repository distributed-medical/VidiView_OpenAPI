namespace VidiView.Api.DataModel;

public class ScheduledStudyCollection
{
    /// <summary>
    /// Number of items in this collection
    /// </summary>
    public int Count { get; init; }

    public DateTime FromDate { get; init; }
    public DateTime ToDateExclusive { get; init; }

    /// <summary>
    /// The configured interval to be used for auto-refresh
    /// </summary>
    public TimeSpan? RefreshInterval { get; init; }

    public int MaximumHits { get; init; }

    /// <summary>
    /// The items
    /// </summary>
    public ScheduledStudy[] Items => Embedded.ScheduledStudies;

    /// <summary>
    /// Any HAL Rest links associated with this collection
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }

    [JsonPropertyName("_embedded")]
    public EmbeddedArray Embedded { get; init; }

    public class EmbeddedArray
    {
        public ScheduledStudy[] ScheduledStudies { get; init; }
    }
}
