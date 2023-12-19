namespace VidiView.Api.DataModel;
public enum ImageFlags
    : long
{
    /// <summary>
    /// Image is hidden
    /// </summary>
    IsHidden = 0x0001,

    /// <summary>
    /// Image is derived
    /// </summary>
    IsDerived = 0x0002,

    /// <summary>
    /// Image is processed
    /// </summary>
    IsProcessed = 0x0004,

    /// <summary>
    /// This image was recorded as a clip (starting from history buffer)
    /// </summary>
    IsClipRecording = 0x0100,

    /// <summary>
    /// This image was recorded from history buffer
    /// </summary>
    IsRecaptureRecording = 0x0200,

    /// <summary>
    /// This image was not finalized correctly, possible due to power failure or application crash
    /// and as much data as possible has been recovered
    /// </summary>
    IsRecovered = 0x1000,

    Rotate90 = 0x1000000,
    Rotate180 = 0x2000000,
    Rotate270 = 0x4000000,
    FlipHorizontal = 0x8000000,
    FlipVertical = 0x10000000,

}
