namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record LoginSession
{
    /// <summary>
    /// User id
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// The device id
    /// </summary>
    public string? DeviceId { get; init; }

    /// <summary>
    /// The device name
    /// </summary>
    public string? DeviceName { get; init; }

    /// <summary>
    /// The application id
    /// </summary>
    public Guid AppId { get; init; }

    /// <summary>
    /// The application name
    /// </summary>
    public string? AppName { get; init; }

    /// <summary>
    /// The application version
    /// </summary>
    public string? AppVersion { get; init; }

    /// <summary>
    /// Login authority
    /// </summary>
    public string? Authority { get; init; }

    /// <summary>
    /// The time of the login
    /// </summary>
    public DateTimeOffset LoginTime { get; init; }

    /// <summary>
    /// The time this login session expires
    /// </summary>
    public DateTimeOffset ExpiryTime { get; init; }

    /// <summary>
    /// Returns true if the session corresponds to the one that made a call to get sessions(s)
    /// </summary>
    public bool IsCurrent { get; init; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }

    public override string ToString()
    {
        return $"{AppName ?? AppId.ToString()} ({Authority}) login time {LoginTime}";
    }
}
