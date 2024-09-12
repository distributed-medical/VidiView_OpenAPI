namespace VidiView.Api.DataModel;

[JsonConverter(typeof(StringEnumConverterEx<TrackableType>))]
public enum TrackableType
{
    Unknown = 0,
    Nevus, // Mole
    Wound,
}
