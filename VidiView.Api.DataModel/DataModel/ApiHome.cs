namespace VidiView.Api.DataModel;

/// <summary>
/// This record represents the information provided by the 
/// VidiView Server when requesting the Api starting address
/// </summary>
public record ApiHome
{
    /// <summary>
    /// The application label (MDR regulation)
    /// </summary>
    public ApplicationLabel Label { get; init; } = null!;

    /// <summary>
    /// The current VidiView Api version
    /// </summary>
    public string ApiVersion { get; init; } = null!;

    /// <summary>
    /// This Api version should be compatible backwards to this version
    /// </summary>
    public string CompatibleApiVersion { get; init; } = null!;

    /// <summary>
    /// The server id
    /// </summary>
    public Guid ServerId { get; init; }
    
    /// <summary>
    /// The server name
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// The server version
    /// </summary>
    public string ServerVersion { get; init; } = null!;

    /// <summary>
    /// The server local time
    /// </summary>
    public DateTimeOffset LocalTime { get; init; }

    /// <summary>
    /// Returns an array of external id providers configured
    /// </summary>
    public IdentityProvider[]? IdentityProviders { get; init; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")] 
    public LinkCollection? Links { get; init; }
}
