namespace VidiView.Api.DataModel;

public record TokenRequest
{
    /// <summary>
    /// The requested lifetime of this token. If null, the default lifetime will be used
    /// </summary>
    public TimeSpan? Lifetime { get; init; }

    /// <summary>
    /// If token is intended for another application, specify
    /// the application id here
    /// </summary>
    public Guid? AppId { get; init; }

    /// <summary>
    /// If set, the returned token will be of one-time-password type
    /// </summary>
    public bool OtpToken { get; init; } = false;

    /// <summary>
    /// Update the scope. 
    /// Note! You cannot extend the scope beyond what is already granted in the current session
    /// </summary>
    public string? Scope { get; init; }

    /// <summary>
    /// If supplied, this will add a contribute restriction with access to the specified studies only
    /// </summary>
    public Guid[]? ContributeStudy { get; init; }

}
