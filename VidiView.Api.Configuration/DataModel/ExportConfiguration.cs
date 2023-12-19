namespace VidiView.Configuration.Api;

public enum AnnotationOption
{
    /// <summary>
    /// Skip annotations when exporting
    /// </summary>
    Skip,

    /// <summary>
    /// Create a Structured report for the annotaions
    /// </summary>
    SR,

    /// <summary>
    /// Burn annotation in image
    /// </summary>
    Burn
}
public record ExportConfiguration
{
    /// <summary>
    /// This archive accepts snapshot images
    /// </summary>
    public bool AcceptSnapshot { get; init; }

    /// <summary>
    /// This archive accepts video images
    /// </summary>
    public bool AcceptVideo { get; init; }

    /// <summary>
    /// Annotation handling
    /// </summary>
    public AnnotationOption Annotations { get; init; } = AnnotationOption.SR;

    /// <summary>
    /// Maximum video duration for export
    /// </summary>
    public TimeSpan MaxVideoDuration { get; init; }

    /// <summary>
    /// Reduce videoresolution to specified number of lines
    /// </summary>
    public int MaxVideoResolution { get; init; }

    /// <summary>
    /// Reduce bit rate to the specified bit rate
    /// </summary>
    /// <remarks>This is defined in Kbit/s</remarks>
    public int MaxVideoBitRate { get; init; }
}
