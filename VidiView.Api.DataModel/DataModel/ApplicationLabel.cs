namespace VidiView.Api.DataModel;

/// <summary>
/// The application label (MDR regulation)
/// </summary>
[ExcludeFromCodeCoverage]
public record ApplicationLabel
{
    /// <summary>
    /// Application name
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Application version
    /// </summary>
    public string? Version { get; init; }

    /// <summary>
    /// Application UDI
    /// </summary>
    public string? UDI { get; init; }

    /// <summary>
    /// Manufacturer's legal name
    /// </summary>
    public string? ManufacturerName { get; init; }

    /// <summary>
    /// Manufacturer's legal address
    /// </summary>
    public string? ManufacturerAddress { get; init; }

    /// <summary>
    /// Application license serial number
    /// </summary>
    public string? SerialNumber { get; init; }

    /// <summary>
    /// MDR notice
    /// </summary>
    public string? Note { get; init; }
}
