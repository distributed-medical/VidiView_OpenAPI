namespace VidiView.Api.DataModel;

/// <summary>
/// This class represents an annotation in an image
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
    public string Type { get; init; } = null!;

    public string? Color { get; init; }

    public int X1 { get; init; }

    public int Y1 { get; init; }

    public int X2 { get; init; }

    public int Y2 { get; init; }

    public int ZIndex { get; init; }

    /// <summary>
    /// Annotation offset from beginning of media file
    /// </summary>
    public TimeSpan? Offset { get; init; }

    public double? Thickness { get; init; }

    public string? Text { get; init; }

    public int? FontSize { get; init; }

    public IdAndName UpdatedBy { get; init; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection Links { get; init; }
}
