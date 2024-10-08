namespace VidiView.Api.DataModel;

public class TokenRequest
{
    /// <summary>
    /// The requested lifetime of this token. If null, the default lifetime will be used
    /// </summary>
    public TimeSpan? Lifetime { get; init; }

    /// <summary>
    /// Update the scope. 
    /// Note! You cannot extend the scope beyond what is already granted in the current session
    /// </summary>
    public string? Scope { get; set; }

    /// <summary>
    /// If token is intended for another application, specify
    /// the application id here
    /// </summary>
    public Guid? AppId { get; set; }

    /// <summary>
    /// If supplied, this will add a contribute restriction with access to the specified studies only
    /// </summary>
    public Guid[]? ContributeStudy { get; set; }

}
