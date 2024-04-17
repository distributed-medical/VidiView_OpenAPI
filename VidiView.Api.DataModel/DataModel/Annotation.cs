namespace VidiView.Api.DataModel;

/// <summary>
/// This record represents an annotation in an image
/// </summary>
public record Annotation
{
    /// <summary>
    /// The unique id
    /// </summary>
    public Guid AnnotationId { get; init; }

    public Guid ImageId { get; init; }

    /// <summary>
    /// Annotation type
    /// </summary>
    public AnnotationType Type { get; init; }

    /// <summary>
    /// Hex color code
    /// </summary>
    public string? Color { get; init; }

    /// <summary>
    /// Points for this annotation
    /// </summary>
    public PointInt[]? Points { get; init; }

    /// <summary>
    /// ZIndex. The higher value is in foreground
    /// </summary>
    public int ZIndex { get; init; }

    /// <summary>
    /// Thickness of annotation
    /// </summary>
    public double? Thickness { get; init; }

    /// <summary>
    /// Optional text
    /// </summary>
    public string? Text { get; init; }

    public int? FontSize { get; init; }

    public IdAndName UpdatedBy { get; init; } = null!;

    /// <summary>
    /// Annotation offset from beginning of media file
    /// </summary>
    public TimeSpan? Offset { get; init; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }
}
