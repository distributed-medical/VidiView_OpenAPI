namespace VidiView.Api.DataModel;

/// <summary>
/// Information about an active conference
/// </summary>
public class Conference
{
    /// <summary>
    /// Id of this conference
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The Controller where this conference is broadcasted from 
    /// </summary>
    public IdAndName Controller { get; set; }

    /// <summary>
    /// The Controller location
    /// </summary>
    public string Location { get; set; }

    /// <summary>
    /// Date and time when the conference was created
    /// </summary>
    public DateTimeOffset ConferenceStartDate { get; init; }

    /// <summary>
    /// Type of conference being active
    /// </summary>
    public ConferenceType ConferenceType { get; init; }

    /// <summary>
    /// Active live sources in this conference
    /// </summary>
    /// <remarks>
    /// This will only be set when the study is
    /// open/active by the user
    /// </remarks>
    public ConferenceSource[]? Sources { get; init; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }
}
