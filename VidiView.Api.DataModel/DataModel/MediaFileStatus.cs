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
    /// File has been erased on the server
    /// </summary>
    FileErased = 80,
}
