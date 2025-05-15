namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record WorklistAssignment : Worklist
{
    /// <summary>
    /// Date/time when study was assigned to worklist
    /// </summary>
    public DateTimeOffset AddedDate { get; init; }

    /// <summary>
    /// Optional comment
    /// </summary>
    public string? Comment { get; init; }
}
