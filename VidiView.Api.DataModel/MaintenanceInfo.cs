namespace VidiView.Api.DataModel;

public record MaintenanceInfo
{
    public bool MaintenanceMode { get; init; }
    public string? Message { get; init; }
    public DateTimeOffset? Until { get; init; }
    public string[]? AlternateHostName { get; init; }
}
