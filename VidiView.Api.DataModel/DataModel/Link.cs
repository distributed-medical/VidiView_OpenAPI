namespace VidiView.Api.DataModel;

/// <summary>
/// Link to other resource (HATEOAS)
/// </summary>
[ExcludeFromCodeCoverage]
public record Link
{
    /// <summary>
    /// The link relation 
    /// </summary>
    [JsonIgnore]
    public string Rel { get; internal set; } = null!; // Since this is not serialized, we need to set this value internally when deserializing

    /// <summary>
    /// The full uri to the resource
    /// </summary>
    /// <remarks>
    /// If <see cref="Templated"/> is true, this Uri contains parameters that 
    /// need to be replaced before a valid Uri can be created
    /// </remarks>
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
