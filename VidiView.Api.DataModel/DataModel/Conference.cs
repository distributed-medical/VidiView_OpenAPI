namespace VidiView.Api.DataModel;

/// <summary>
/// Information about an active conference
/// </summary>
public class Conference
{
    /// <summary>
    /// Date and time when the conference was created
    /// </summary>
    public DateTimeOffset ConferenceStartDate { get; init; }

    /// <summary>
    /// Type of conference being active
    /// </summary>
    public ConferenceType ConferenceType { get; init; }

    /// <summary>
    /// Active live sources in this conference (when the
    /// <see cref="ConferenceType"/> is LiveConference
    /// </summary>
    /// <remarks>
    /// This will only be set when the study is
    /// open/active by the user
    /// </remarks>
    public LiveConferenceSource[]? LiveSources { get; init; }

}
