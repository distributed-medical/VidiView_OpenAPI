namespace VidiView.Api.DataModel;

/// <summary>
/// Link to other resource (HATEOAS)
/// </summary>
[ExcludeFromCodeCoverage]
public record Link
{
    private string? _rel;

    public Link()
    {
    }

    /// <summary>
    /// This is mostly intended for testing purposes
    /// </summary>
    /// <param name="rel"></param>
    /// <param name="href"></param>
    /// <param name="templated"></param>
    public Link(string rel, string href, bool templated = false)
    {
        Rel = rel;
        Href = href;
        Templated = templated;
    }

    /// <summary>
    /// The link relation 
    /// </summary>
    [JsonIgnore]
    public string Rel 
    { 
        get => _rel ?? throw new InvalidOperationException("Rel must be set before using the Link object.");

        // Since this is not serialized,
        // we need to set this value internally
        // when deserializing.
        internal set
        { 
            if (_rel != null)
            {
                throw new InvalidOperationException("Rel can only be set once.");
            }
            _rel = value;
        } 
    } 

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
