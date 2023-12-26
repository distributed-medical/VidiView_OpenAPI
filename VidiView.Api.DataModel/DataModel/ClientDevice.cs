namespace VidiView.Api.DataModel;

public record ClientDevice
{
    /// <summary>
    /// The app id
    /// </summary>
    public string AppId { get; init; }

    /// <summary>
    /// Application version
    /// </summary>
    public string AppVersion { get; init; }

    /// <summary>
    /// The device id
    /// </summary>
    /// <remarks>This might not be the same as the DeviceId supplied in the Api-key header</remarks>
    public string DeviceId { get; init; }

    /// <summary>
    /// Device model
    /// </summary>
    public string? Model { get; init; }

    /// <summary>
    /// Operating system on which the client is running
    /// </summary>
    public string OSVersion { get; init; }

    /// <summary>
    /// This token is used to manually identify
    /// a device during registration/grant
    /// </summary>
    public string? RegistrationToken { get; init; }

    /// <summary>
    /// The date and time this device was registered
    /// </summary>
    public DateTimeOffset? RegistrationDate { get; init; }

    /// <summary>
    /// Returns true if this device is granted access to VidiView
    /// </summary>
    public bool IsGranted { get; init; }

    public ClientDeviceFlags Flags { get; init; }

    /// <summary>
    /// Device name (i.e. computer name)
    /// </summary>
    public string? DeviceName { get; init; }

    /// <summary>
    /// Device name, specified by user
    /// </summary>
    public string? DeviceNameOverride { get; init; }
}
