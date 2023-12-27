namespace VidiView.Api.DataModel;

/// <summary>
/// Extends the study record with properties assigned to personal worklist items
/// </summary>
public record BookmarkedStudy : Study
{
    /// <summary>
    /// User's comment
    /// </summary>
    public string? UserComment { get; init; }

    /// <summary>
    /// The time this study was added to the personal worklist
    /// </summary>
    public DateTimeOffset UserAddedDate { get; init; }
}
