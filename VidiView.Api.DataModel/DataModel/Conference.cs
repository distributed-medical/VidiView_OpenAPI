namespace VidiView.Api.DataModel;

/// <summary>
/// Information about an active conference
/// </summary>
[ExcludeFromCodeCoverage]
public record Conference
{
    /// <summary>
    /// Id of this conference
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The patient for which this conference is active 
    /// </summary>
    /// <remarks>Null for unidentified patients</remarks>
    public Guid? PatientId { get; set; }

    /// <summary>
    /// Study id
    /// </summary>
    public Guid StudyId { get; init; }

    /// <summary>
    /// The Controller where this conference is broadcasted from 
    /// </summary>
    public IdAndName? Controller { get; init; }

    /// <summary>
    /// The Controller location
    /// </summary>
    public string? Location { get; init; }

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
