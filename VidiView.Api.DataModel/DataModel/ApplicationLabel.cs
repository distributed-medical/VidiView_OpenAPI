namespace VidiView.Api.DataModel;

/// <summary>
/// The application label
/// </summary>
[ExcludeFromCodeCoverage]
public record ApplicationLabel
{
    public string? Name { get; init; }
    public string? Version { get; init; }
    public string? UDI { get; init; }
    public string? ManufacturerName { get; init; }
    public string? ManufacturerAddress { get; init; }
    public string? SerialNumber { get; init; }
    public string? Note { get; init; }
}
