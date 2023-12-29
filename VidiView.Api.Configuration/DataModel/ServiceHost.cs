namespace VidiView.Api.Configuration.DataModel;

public record ServiceHost : VidiView.Api.DataModel.ServiceHost
{
    /// <summary>
    /// Configuration id
    /// </summary>
    public Guid? ConfigurationId { get; init; }

    /// <summary>
    /// Dicom connection parameters, if applicable
    /// </summary>
    public DicomConnection? DicomConnection { get; init; }

    /// <summary>
    /// Export configuration, if applicable
    /// </summary>
    public ExportConfiguration? ExportConfiguration { get; init; }
}
