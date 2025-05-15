namespace VidiView.Api.DataModel;

/// <summary>
/// Execute a remote record command
/// </summary>
[ExcludeFromCodeCoverage]
public record RecordCommand
{
    /// <summary>
    /// Enable or disable video recording
    /// </summary>
    public bool? RecordVideo { get; init; }
}
