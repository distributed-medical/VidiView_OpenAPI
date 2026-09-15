namespace VidiView.Api.DataModel;

/// <summary>
/// The microphone sensitivity to use in a specific media file, to determine the actual sound pressure level (SPL) in dB SPL of the contained audio signal
/// </summary>
[ExcludeFromCodeCoverage]
public record VoiceRecordingSpecifics
{
    /// <summary>
    /// This is the intended media type for this specific data, i.e. the media type that should be used when storing this data in a media file
    /// </summary>
    public static Guid IntendedMediaType => WellKnownMediaType.VoiceRecording;

    /// <summary>
    /// The media file id that contains the microphone calibration data
    /// </summary>
    public Guid CalibrationId { get; init; }

    /// <summary>
    /// Microphone sensitivity, in mV/Pa
    /// </summary>
    public double Sensitivity { get; init; }
}
