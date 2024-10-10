namespace VidiView.Api.DataModel;

public record CameraControl
{
    /// <summary>
    /// Remote control capabilities
    /// </summary>
    public CameraCapabilities Capabilities { get; init; }

    /// <summary>
    /// Predefined presets
    /// </summary>
    public IdAndName[] Presets { get; init; }
}
