namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record AnatomicRegion
{
    /// <summary>
    /// The body map id that this entity is referring to
    /// </summary>
    public Guid MapId { get; init; }

    /// <summary>
    /// An optional array of markers to be overlayed on the anatomic map
    /// </summary>
    /// <remarks>
    /// The point is defined in normalized coordinates relative the actual map.
    /// The map definitions are kept in a separate Nuget package
    /// </remarks>
    public NormalizedPoint[]? Markers { get; init; }

    /// <summary>
    /// The locations described by this anatomic region.
    /// </summary>
    /// <remarks>Each location is defined as a Snomed CT pre- or post coordinated expression</remarks>
    public string[]? Locations { get; init; }

    /// <summary>
    /// The Snomed CT expression
    /// </summary>
    public Snomed? Snomed { get; init; }

    /// <summary>
    /// The raw XML value of this anatomic region
    /// </summary>
    /// <remarks>This must be used to update the position</remarks>
    public string? Xml { get; init; }

}
