namespace VidiView.Api.DataModel;

/// <summary>
/// Represents a live conference source
/// </summary>
public record LiveConferenceSource
{
    /// <summary>
    /// The source Id
    /// </summary>
    public string Id { get; init; }

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
    /// The Controller where this source is broadcasted from 
    /// </summary>
    public IdAndName? Controller { get; init; }

    /// <summary>
    /// The Controller location
    /// </summary>
    public string? Location { get; init; }

    /// <summary>
    /// Associated links
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }
}
