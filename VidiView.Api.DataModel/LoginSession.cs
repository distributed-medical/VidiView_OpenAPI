namespace VidiView.Api.DataModel;

public class LoginSession
{
    /// <summary>
    /// User id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// The device id
    /// </summary>
    public string DeviceId { get; set; }

    /// <summary>
    /// The device name
    /// </summary>
    public string DeviceName { get; set; }

    /// <summary>
    /// The application id
    /// </summary>
    public Guid AppId { get; set; }

    /// <summary>
    /// The application name
    /// </summary>
    public string AppName { get; set; }

    /// <summary>
    /// The application version
    /// </summary>
    public string AppVersion { get; set; }

    /// <summary>
    /// Login authority
    /// </summary>
    public string Authority { get; set; }

    /// <summary>
    /// The time of the login
    /// </summary>
    public DateTimeOffset LoginTime { get; set; }

    /// <summary>
    /// The time this login expires
    /// </summary>
    public DateTimeOffset ExpiryTime { get; set; }

    /// <summary>
    /// Returns true if the session corresponds to the one that made a call to get sessions(s)
    /// </summary>
    public bool IsCurrent { get; set; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection Links { get; set; }

    public override string ToString()
    {
        return $"{AppName ?? AppId.ToString()} ({Authority}) login time {LoginTime}";
    }
}
