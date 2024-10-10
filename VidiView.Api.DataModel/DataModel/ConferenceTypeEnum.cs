namespace VidiView.Api.DataModel;

[JsonConverter(typeof(StringEnumConverterEx<ConferenceType>))]
public enum ConferenceType
{
    /// <summary>
    /// No active conference
    /// </summary>
    None = 0,

    /// <summary>
    /// A live conference from a legacy Controller
    /// </summary>
    LegacyConference = 1,
}
