namespace VidiView.Api.DataModel;

/// <summary>
/// Link to other resource (HATEOAS)
/// </summary>
public record Link
{
    [JsonIgnore]
    public string Rel { get; init; } = null!;

    /// <summary>
    /// The full uri to the resource
    /// </summary>
    public string Href { get; init; } = null!;

    /// <summary>
    /// True if Href is templated (RFC 6570)
    /// </summary>
    public bool Templated { get; init; } 

    /// <summary>
    /// An optional human readable title of the link representation
    /// </summary>
    public string? Title { get; init; } 

    /// <summary>
    /// The content-type to expect when fetching this resource
    /// </summary>
    public string? Type { get; init; }

    public override string ToString()
    {
        return $"{Rel} {Href}".Trim();
    }
}
