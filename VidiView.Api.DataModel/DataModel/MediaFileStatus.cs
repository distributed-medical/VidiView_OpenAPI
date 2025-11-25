namespace VidiView.Api.DataModel;

[JsonConverter(typeof(StringEnumConverterEx<MediaFileStatus>))]
public enum MediaFileStatus
{
    /// <summary>
    /// Fallback value for enum values unknown to the API
    /// </summary>
    Unknown = -1,

    /// <summary>
    /// Image metadata announced, 
    /// </summary>
    Announced = 0,

    /// <summary>
    /// Image file received and verified
    /// </summary>
    Verified = 20,

    /// <summary>
    /// This is an image that has been received from an external system (modality receive)
    /// </summary>
    ModalityReceive = 62,

    /// <summary>
    /// Image is exported but not present in this system
    /// </summary>
    ExportedNotPresent = 70,

    /// <summary>
    /// Image is exported and restored from archive
    /// </summary>
    ExportedRestoredFromArchive = 71,

    /// <summary>
    /// File has been erased on the server
    /// </summary>
    FileErased = 80,
}
