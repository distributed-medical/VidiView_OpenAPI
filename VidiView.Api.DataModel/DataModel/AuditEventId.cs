namespace VidiView.Api.DataModel;

/// <summary>
/// Event id information
/// </summary>
[ExcludeFromCodeCoverage]
public record AuditEventId
{
    /// <summary>
    /// Event id
    /// </summary>
    public int EventId { get; init; }

    /// <summary>
    /// Event category
    /// </summary>
    public string Category { get; init; }

    /// <summary>
    /// Event name
    /// </summary>
    public string Name { get; init; }
}
