namespace VidiView.Api.DataModel;

[JsonConverter(typeof(StringEnumConverterEx<MimeTypeFlags>))]
[Flags]
public enum MimeTypeFlags : long
{
    None = 0x0,

    /// <summary>
    /// Allow upload of files with this mime type from VidiView Client
    /// </summary>
    AllowUpload = 0x1,

    /// <summary>
    /// The server can generate thumbnails for image or video files with mime type.
    /// The client can request a thumbnail for a file with this mime type.
    /// </summary>
    Thumbnail = 0x100,

    /// <summary>
    /// The server can extract a frame from video files with this mime type.
    /// The client can request a frame extraction for a file with this mime type.
    /// </summary>
    ExtractFrame = 0x200,

    /// <summary>
    /// The server supports trimming of video files with this mime type. 
    /// The client can request a trimmed version of the video file.
    /// </summary>
    Trim = 0x400,

    /// <summary>
    /// Send image availability notifications when a file with this mime type is uploaded or deleted. 
    /// This is used to support IAN workflow to notify external systems of files being present in VidiView.
    /// </summary>
    SendIan = 0x1000,

    /// <summary>
    /// Prohibit file from being erased. This is used to protect files 
    /// that are required for the system to function properly.
    /// </summary>
    ProhibitErase = 0x2000, // This is handled in [usp_ImagesToErase]
}