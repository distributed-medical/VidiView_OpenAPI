namespace VidiView.Api.DataModel;

public class UserAndClient
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