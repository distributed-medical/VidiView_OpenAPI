namespace VidiView.Api.DataModel;

/// <summary>
/// Represents a live conference source
/// </summary>
[ExcludeFromCodeCoverage]
public record ConferenceSource
{
    /// <summary>
    /// The source Id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The source name, i.e Endoscope
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// The source input type, i.e SDI
    /// </summary>
    public string InputType { get; init; } = string.Empty;

    /// <summary>
    /// Additional video information
    /// </summary>
    public VideoInformation VideoInformation { get; init; }

    /// <summary>
    /// If camera is remote controllable, this defines
    /// the supported capabilities
    /// </summary>
    public CameraControl? Controllable { get; init; }

    /// <summary>
    /// Flags for this source
    /// </summary>
    public ConferenceSourceFlags Flags { get; init; }

    /// <summary>
    /// Time when the current recording started
    /// </summary>
    public DateTimeOffset? RecordingStarted { get; init; }

    /// <summary>
    /// Associated links
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }
}
