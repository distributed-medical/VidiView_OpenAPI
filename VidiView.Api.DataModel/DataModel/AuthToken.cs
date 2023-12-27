namespace VidiView.Api.DataModel;

public record AuthToken
{
    /// <summary>
    /// The id of this token
    /// </summary>
    [JsonPropertyName("assertion-id")]
    public string Id { get; init; }

    /// <summary>
    /// The time when this token expires
    /// </summary>
    public DateTimeOffset Expires { get; init; }

    /// <summary>
    /// The token itself
    /// </summary>
    public string Token { get; init; }

    /// <summary>
    /// Returns the current pseudonymization state
    /// </summary>
    public bool Pseudonymize { get; init; } = false;

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")] 
    public LinkCollection? Links { get; init; }


}
