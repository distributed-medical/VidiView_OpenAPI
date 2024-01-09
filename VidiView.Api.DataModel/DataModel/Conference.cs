namespace VidiView.Api.DataModel;

/// <summary>
/// Active conference
/// </summary>
public record Conference
{
    /// <summary>
    /// StudyId for which this conference is active
    /// </summary>
    public Guid StudyId { get; init; }

    /// <summary>
    /// Study created date
    /// </summary>
    public DateTimeOffset StudyDate { get; init; }

    /// <summary>
    /// Study department
    /// </summary>
    public IdAndName Department { get; init; }

    /// <summary>
    /// Study patient
    /// </summary>
    public Patient Patient { get; init; }

    /// <summary>
    /// The time this conference was started
    /// </summary>
    public DateTimeOffset ConferenceStartTime { get; init; }

    /// <summary>
    /// Active VidiView Controller sources in this conference
    /// </summary>
    public ConferenceSource[] ControllerSources { get; set; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }

}
