namespace VidiView.Api.DataModel;

public record TrustedIssuer
{
    public string Thumbprint { get; init; } = null!;

    public string DistinguishedName { get; init; } = null!;

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")] 
    public LinkCollection? Links { get; init; }
}
