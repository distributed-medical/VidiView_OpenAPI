namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record AnatomicRegion
{
    /// <summary>
    /// The body map id that this entity is referring to
    /// </summary>
    public Guid MapId { get; init; }

    /// <summary>
    /// The X coordinate indicated (normalized)
    /// </summary>
    public double X { get; init; }

    /// <summary>
    /// The Y coordinate indicated (normalized)
    /// </summary>
    public double Y { get; init; }

    /// <summary>
    /// The Snomed CT expression
    /// </summary>
    public Snomed Snomed { get; init; } = null!;

    /// <summary>
    /// The raw XML value of this anatomic region
    /// </summary>
    /// <remarks>This must be used to update the position</remarks>
    public string? Xml { get; init; } 
}
