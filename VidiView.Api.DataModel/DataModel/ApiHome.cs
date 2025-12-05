namespace VidiView.Api.DataModel;

/// <summary>
/// This record represents the information provided by the 
/// VidiView Server when requesting the Api starting address
/// </summary>
[ExcludeFromCodeCoverage]
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
    /// This is an indication to the client of which application version
    /// is intended to be used to access the VidiView Server
    /// </summary>
    /// <remarks>The implementation is client specific</remarks>
    public VersionRange? IntendedClientApplicationVersion { get; init; }

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
    /// Returns application startup time
    /// </summary>
    /// <remarks>Only returned by configuration API</remarks>
    public DateTimeOffset? ApplicationStartupTime { get; set; }

    /// <summary>
    /// Returns license status
    /// </summary>
    /// <remarks>Only returned by configuration API</remarks>
    public string? LicenseStatus { get; set; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")] 
    public LinkCollection? Links { get; init; }
}
