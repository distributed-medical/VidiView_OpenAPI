namespace VidiView.Api.DataModel;

[JsonConverter(typeof(StringEnumConverterEx<ConferenceSourceFlags>))]
[Flags]
public enum ConferenceSourceFlags
{
    None = 0x0,

    /// <summary>
    /// Selected for photo capture
    /// </summary>
    Photo = 0x10,

    /// <summary>
    /// Selected for video recording
    /// </summary>
    Video = 0x20,

    /// <summary>
    /// Selected for audio/video recording
    /// </summary>
    Audio = 0x40,

    /// <summary>
    /// Broadcast enabled
    /// </summary>
    Broadcast = 0x80,

    /// <summary>
    /// Is currently recording video
    /// </summary>
    Recording = 0x1000,

    /// <summary>
    /// Is currently recording a video clip
    /// </summary>
    ClipRecording = 0x2000
}
