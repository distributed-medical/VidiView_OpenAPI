namespace VidiView.Api.DataModel;

public record ServiceHost
{
    /// <summary>
    /// The service host id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The service host name
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Service host type
    /// </summary>
    public ServiceHostType Type { get; init; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }

    public override string ToString() => $"{Type} ({Name})";
}
