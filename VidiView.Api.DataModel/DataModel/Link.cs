namespace VidiView.Api.DataModel;

/// <summary>
/// Link to other resource (HATEOAS)
/// </summary>
public record Link
{
    [JsonIgnore]
    public string Rel { get; set; } = null!;

    /// <summary>
    /// The full uri to the resource
    /// </summary>
    public string Href { get; set; } = null!;

    /// <summary>
    /// True if Href is templated (RFC 6570)
    /// </summary>
    public bool Templated { get; set; } 

    /// <summary>
    /// An optional human readable title of the link representation
    /// </summary>
    public string? Title { get; set; } 

    /// <summary>
    /// The content-type to expect when fetching this resource
    /// </summary>
    public string? Type { get; set; }

    public override string ToString()
    {
        return $"{Rel} {Href}".Trim();
    }
}
