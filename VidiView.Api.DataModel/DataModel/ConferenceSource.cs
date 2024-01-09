namespace VidiView.Api.DataModel;

/// <summary>
/// Represents a live conference source
/// </summary>
public record ConferenceSource
{
    /// <summary>
    /// The source name
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// The source input type
    /// </summary>
    public string InputType { get; init; } = null!;

    /// <summary>
    /// The Controller where this source is broadcasted from 
    /// </summary>
    public Guid? ControllerId { get; init; }

    /// <summary>
    /// The source location
    /// </summary>
    public string? Location { get; init; }

    /// <summary>
    /// Information about this source
    /// </summary>
    public string? StreamResolution { get; init; }

    /// <summary>
    /// Associated links
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }
}
