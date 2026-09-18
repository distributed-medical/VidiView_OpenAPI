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
    [JsonIgnore]
    public static Guid IntendedMediaType => WellKnownMediaType.VoiceRecording;

    /// <summary>
    /// The media file id that contains the microphone calibration data
    /// </summary>
    public Guid CalibrationId { get; init; }

    /// <summary>
    /// The time when the microphone was calibrated, in UTC
    /// </summary>
    public DateTimeOffset CalibrationDate { get; init; }

    /// <summary>
    /// The calibration factor (C) to convert measured level (Lm) to sound pressure level (Lspl), Lspl = Lm ​+ C
    /// </summary>
    public double CalibrationFactor { get; init; }

    /// <summary>
    /// The sensitivity of the microphone in Pa/FS (Pascal relative to full scale)
    /// </summary>
    public double Sensitivity { get; init; }
}
