namespace VidiView.Api.DataModel;

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
    public Snomed Snomed { get; init; }
}
