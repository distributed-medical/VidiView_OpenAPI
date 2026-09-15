namespace VidiView.Api;

/// <summary>
/// These are well-known media types, that are defined by the manufacturer.
/// These are used to add specific behavior in different applications
/// </summary>
public static class WellKnownMediaType
{
    /// <summary>
    /// This media is a forensic odontology, taken after time of death (post-mortem)
    /// </summary>
    public static readonly Guid ForensicOdontologyPostMortem = new Guid("AD3AAE38-FD10-4811-B96A-EC9C4857FB41");

    /// <summary>
    /// This media is a forensic odontology, taken before time of death (ante-mortem)
    /// </summary>
    public static readonly Guid ForensicOdontologyAnteMortem = new Guid("088457AB-5C54-444F-8ACD-FDE3E7CFE0A2");

    /// <summary>
    /// This media is a voice recording, which can be used for speech and language assessment
    /// </summary>
    public static readonly Guid VoiceRecording = new Guid("9B0A8563-CED4-46D8-A803-D184927BC589");

    /// <summary>
    /// This media is a microphone sound pressure level calibration
    /// </summary>
    public static readonly Guid MicrophoneSoundPressureLevelCalibration = new Guid("A769B55B-ACD7-421E-A800-B5143959F23D");

}
