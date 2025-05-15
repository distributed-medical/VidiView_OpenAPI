namespace VidiView.Api.DataModel;

/// <summary>
/// This record represents a trackable find
/// </summary>
[ExcludeFromCodeCoverage]
public record Trackable
{
    /// <summary>
    /// The unique id
    /// </summary>
    public Guid TrackableId { get; init; }

    /// <summary>
    /// The patient this trackable is associated with
    /// </summary>
    public Patient Patient { get; init; }

    /// <summary>
    /// Type
    /// </summary>
    public TrackableType Type { get; init; }

    /// <summary>
    /// Optional name
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Optional dscription
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Anatomic region for this trackable find
    /// </summary>
    public AnatomicRegion? AnatomicRegion { get; init; }

    /// <summary>
    /// A photo (media file) that identifies this trackable find
    /// </summary>
    public Guid? PhotoId { get; init; }

    /// <summary>
    /// An annotation that identifies this trackable find
    /// </summary>
    public Guid? AnnotationId { get; init; }

    /// <summary>
    /// Timestamp value that changes with any update
    /// </summary>
    public string Timestamp { get; init; }

    ///// <summary>
    ///// The user who last updated this item
    ///// </summary>
    //public IdAndName UpdatedBy { get; init; } = null!;

    ///// <summary>
    ///// The date and time this trackable was last updated
    ///// </summary>
    //public DateTimeOffset UpdatedDate { get; init; }

    /// <summary>
    /// If this is set, the trackable is deleted
    /// </summary>
    public DateTimeOffset? DeletedDate { get; init; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }
}
