namespace VidiView.Api.DataModel;

[JsonConverter(typeof(StringEnumConverterEx<ConferenceType>))]
public enum ConferenceType
{
    Unknown = 0,

    /// <summary>
    /// No active conference
    /// </summary>
    None = 1,

    /// <summary>
    /// A live conference from a legacy Controller
    /// </summary>
    LiveConference = 2,
}
