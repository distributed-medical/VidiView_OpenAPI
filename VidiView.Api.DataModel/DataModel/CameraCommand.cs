namespace VidiView.Api.DataModel;

/// <summary>
/// Execute a camera command
/// </summary>
public record CameraCommand
{
    /// <summary>
    /// Select a specific preset
    /// </summary>
    public Guid? PresetId { get; init; }

    /// <summary>
    /// Pan camera relative current position and visible area
    /// </summary>
    /// <remarks>Current position is 0.5, the left edge of the visible area is 0.0 and the right edge is 1.0</remarks>
    public double? PanRelative { get; init; }

    /// <summary>
    /// Pan camera to absolute position
    /// </summary>
    /// <remarks>Leftmost position of camera is 0.0 and rightmost 1.0</remarks>
    public double? PanAbsolute { get; init; }

    /// <summary>
    /// Tilt camera relative current position and visible area
    /// </summary>
    /// <remarks>Current position is 0.5, the top edge of the visible area is 1.0 and the bottom edge is 0.0</remarks>
    public double? TiltRelative { get; init; }

    /// <summary>
    /// Tilt camera to absolute position
    /// </summary>
    /// <remarks>Top position of camera is 1.0 and bottom is 0.0</remarks>
    public double? TiltAbsolute { get; init; }

    /// <summary>
    /// Set camera zoom to absolute value.
    /// </summary>
    /// <remarks>Minimum zoom is 0.0 (wide), and maximum supported zoom is 1.0 (tele)</remarks>
    public double? ZoomAbsolute { get; init; }

    /// <summary>
    /// Start/stop a pan/tilt/zoom operation
    /// </summary>
    public CameraDriveEnum? Drive { get; init; }
}
