namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public class ScheduledStudyCollection
{
    /// <summary>
    /// Number of items in this collection
    /// </summary>
    public int Count { get; init; }

    /// <summary>
    /// The queried date
    /// </summary>
    public DateTime Date { get; init; }

    /// <summary>
    /// Gets the timestamp indicating when the instance was created or initialized.
    /// </summary>
    public DateTimeOffset InstanceTime { get; init; }

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
