namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record MaintenanceInfo
{
    public bool MaintenanceMode { get; init; }
    public string? Message { get; init; }
    public DateTimeOffset? Until { get; init; }
    public string[]? AlternateHostName { get; init; }
}
