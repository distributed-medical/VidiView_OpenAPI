namespace VidiView.Api.DataModel;

/// <summary>
/// Specialized information for export queue
/// </summary>
[ExcludeFromCodeCoverage]
public record ExportQueue : ServiceHost
{
    /// <summary>
    /// Number of items tentatively added to queue (export when study is released)
    /// </summary>
    public int? TentativeCount { get; init; }

    /// <summary>
    /// Number of items ready to be transferred or waiting for conversion etc (excludes tentative)
    /// </summary>
    public int? WaitingCount { get; init; }

    /// <summary>
    /// Number of items in progress
    /// </summary>
    public int? InProgressCount { get; init; }

    /// <summary>
    /// Number of items failed transfer
    /// </summary>
    public int? FailedCount { get; init; }

    /// <summary>
    /// Number of items successfully transferred today
    /// </summary>
    public int? SuccessTodayCount { get; init; }
}
