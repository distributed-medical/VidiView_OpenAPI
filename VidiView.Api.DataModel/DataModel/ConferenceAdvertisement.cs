namespace VidiView.Api.DataModel;

public class ConferenceAdvertisement
{
    public ConferenceType ConferenceType { get; init; }
    public IdAndName Department { get; init; }
    public Guid StudyId { get; init; }
    public Guid? PatientId { get; init; }
}
