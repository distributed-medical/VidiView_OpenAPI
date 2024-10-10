namespace VidiView.Api.DataModel;

public class ConferenceAdvertisement
{
    public ConferenceType ConferenceType { get; init; }
    public IdAndName Department { get; init; }
    public Guid StudyId { get; init; }
    public Guid? PatientId { get; init; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }
}
