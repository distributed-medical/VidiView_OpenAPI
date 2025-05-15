namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record AuditEventCollection
{
    /// <summary>
    /// Number of items in this collection
    /// </summary>
    public int Count { get; init; }

    public DateTime FromDate { get; init; }
    public DateTime ToDateExclusive { get; init; }

    /// <summary>
    /// The events
    /// </summary>
    public AuditEvent[] Items => Embedded.Events;

    /// <summary>
    /// Any HAL Rest links associated with this collection
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }

    [JsonPropertyName("_embedded")]
    public EmbeddedArray Embedded { get; init; }

    public record EmbeddedArray
    {
        public AuditEvent[] Events { get; init; }
    }
}
