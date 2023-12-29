using VidiView.Api.DataModel;

namespace VidiView.Api.Configuration.DataModel;

public record ServiceHost
{
    /// <summary>
    /// The service host id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The service host name
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Service host type
    /// </summary>
    public ServiceHostType Type { get; init; }

    /// <summary>
    /// Configuration id
    /// </summary>
    public Guid? ConfigurationId { get; init; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection Links { get; init; }

    /// <summary>
    /// Dicom connection parameters, if applicable
    /// </summary>
    public DicomConnection? DicomConnection { get; init; }

    /// <summary>
    /// Export configuration, if applicable
    /// </summary>
    public ExportConfiguration? ExportConfiguration { get; init; }

    public override string ToString() => $"{Type} ({Name})";
}
