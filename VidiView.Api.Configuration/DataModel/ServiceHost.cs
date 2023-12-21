using VidiView.Api.DataModel;

namespace VidiView.Configuration.Api;

public class ServiceHost
{
    /// <summary>
    /// The service host id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The service host name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Service host type
    /// </summary>
    public ServiceHostType Type { get; set; }

    /// <summary>
    /// Configuration id
    /// </summary>
    public Guid? ConfigurationId { get; set; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection Links { get; set; }

    /// <summary>
    /// Dicom connection parameters, if applicable
    /// </summary>
    public DicomConnection? DicomConnection { get; set; }

    /// <summary>
    /// Export configuration, if applicable
    /// </summary>
    public ExportConfiguration? ExportConfiguration { get; set; }

    public override string ToString() => $"{Type} ({Name})";
}
