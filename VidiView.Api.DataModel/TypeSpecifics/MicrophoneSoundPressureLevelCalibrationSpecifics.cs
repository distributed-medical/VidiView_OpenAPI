namespace VidiView.Api.DataModel;

/// <summary>
/// Specifics used when calibrating a microphone to determine its sensitivity
/// </summary>
[ExcludeFromCodeCoverage]
public record MicrophoneSoundPressureLevelCalibrationSpecifics
{
    /// <summary>
    /// This is the intended media type for this specific data, i.e. the media type that should be used when storing this data in a media file
    /// </summary>
    [JsonIgnore]
    public static Guid IntendedMediaType => WellKnownMediaType.MicrophoneSoundPressureLevelCalibration;

    /// <summary>
    /// Signal used for calibration, e.g. "Sine wave 1kHz", "Pink noise" etc
    /// </summary>
    public string CalibrationSignal { get; init; }

    /// <summary>
    /// Reference level for calibration, in dB SPL, e.g. 94.0, 114.0 
    /// </summary>
    public double ReferenceLevel { get; init; }

    /// <summary>
    /// Id of the microphone being calibrated
    /// </summary>
    public string MicrophoneId { get; init; }

    /// <summary>
    /// Sample rate used when measuring the microphone calibration, in Hz, e.g. 44100.0, 48000.0
    /// </summary>
    public double SampleRate { get; init; }

    /// <summary>
    /// The device volume level (Windows volume level) used when measuring the microphone calibration (0.0 - 100.0)
    /// </summary>
    public double VolumeLevel { get; init; }

    /// <summary>
    /// The offset from the start of the media file where the calibration measurement starts.
    /// </summary>
    public TimeSpan Offset { get; init; }

    /// <summary>
    /// The duration of the calibration measurement
    /// </summary>
    public TimeSpan Duration { get; init; }

    /// <summary>
    /// The calculated sensitivity of the microphone, in FS/Pa (full-scale/Pascal)
    /// </summary>
    public double Sensitivity { get; init; }
}
