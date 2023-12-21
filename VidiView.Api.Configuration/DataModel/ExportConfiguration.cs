namespace VidiView.Configuration.Api;

public class ExportConfiguration
{
    /// <summary>
    /// This archive accepts snapshot images
    /// </summary>
    public bool AcceptSnapshot { get; set; }

    /// <summary>
    /// This archive accepts video images
    /// </summary>
    public bool AcceptVideo { get; set; }

    public string Annotations { get; set; }

    /// <summary>
    /// Maximum video duration for export
    /// </summary>
    public TimeSpan MaxVideoDuration { get; set; }

    /// <summary>
    /// Reduce videoresolution to specified number of lines
    /// </summary>
    public int MaxVideoResolution { get; set; }

    /// <summary>
    /// Reduce bit rate to the specified bit rate
    /// </summary>
    /// <remarks>This is defined in Kbit/s</remarks>
    public int MaxVideoBitRate { get; set; }
}
