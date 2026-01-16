namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record ServiceHost
{
    public static implicit operator IdAndName?(ServiceHost? host)
    {
        return host == null ? null : new IdAndName(host.Id, host.Name);
    }

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
    /// If this host represents a Dicom service, these are the configuration options
    /// </summary>
    public ServiceHostDicomConnection? DicomConnection { get; init; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }

    public override string ToString() => $"{Type} ({Name})";
}
