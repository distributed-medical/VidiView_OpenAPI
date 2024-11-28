namespace VidiView.Api.DataModel;

/// <summary>
/// Info about an actor (performing an operation) in the system
/// </summary>
/// <remarks>An actor might be a user, a Controller or the system itself</remarks>
public record Actor
{
    /// <summary>
    /// User currently reviewing this study
    /// </summary>
    public IdAndName User { get; init; }

    /// <summary>
    /// Device used to review study
    /// </summary>
    public ClientDevice Device { get; init; }
}