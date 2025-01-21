namespace VidiView.Api.Configuration.DataModel;
public record ExportConfiguration
{
    /// <summary>
    /// The content types accepted. May use wildcard (i.e. image/ or video/)
    /// </summary>
    public string[]? ContentType { get; init; }

    /// <summary>
    /// Annotation handling
    /// </summary>
    public AnnotationOption Annotations { get; init; } = AnnotationOption.SR;

    /// <summary>
    /// Maximum video duration for export
    /// </summary>
    /// <remarks>If TimeSpan.Zero, no limit will be enforced</remarks>
    public TimeSpan MaxVideoDuration { get; init; }

    /// <summary>
    /// Reduce video resolution to specified number of lines
    /// </summary>
    /// <remarks>If 0, no reduction will be enforced</remarks>
    public int MaxVideoResolution { get; init; }

    /// <summary>
    /// Reduce bit rate to the specified bit rate
    /// </summary>
    /// <remarks>This is defined in Kbit/s. If 0, no reduction will be enforced</remarks>
    public int MaxVideoBitRate { get; init; }

    /// <summary>
    /// Enable legacy large file support. This will generate non-conformant Dicom files
    /// </summary>
    public bool LegacyLargeFileSupport { get; init; }
}
