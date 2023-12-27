namespace VidiView.Api.DataModel;

/// <summary>
/// The Image contains data for a specific image
/// </summary>
public class MediaFile
{
    public string Uri { get; set; }


    [JsonPropertyName("acquisition-date")]
    public DateTimeOffset AcquisitionTime { get; set; }

    /// <summary>
    /// SHA256 checksum of image file. 
    /// Byte array sent as a hex string (2 chars per byte)
    /// </summary>
    public string? Checksum { get; set; }
    /// <summary>
    /// Mime type
    /// </summary>
    public string ContentType { get; set; } = null!;

    /// <summary>
    /// Id of Controller where this image was acquired
    /// </summary>
    public Guid? ControllerId { get; set; }

    /// <summary>
    /// Id of user creating this image
    /// </summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>
    /// If this is set, the image is deleted
    /// </summary>
    public DateTimeOffset? DeletedDate { get; set; }

    /// <summary>
    /// Id of image this image was derived from
    /// </summary>
    public Guid? DerivedFrom { get; set; }

    /// <summary>
    /// Image description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Id of device where this image was acquired
    /// </summary>
    public Guid? DeviceId { get; set; }

    public TimeSpan? Duration { get; set; }

    public ImageFlags Flags { get; set; }

    public string Filename { get; set; } = null!;

    public long FileSize { get; set; }

    public int Height { get; set; }

    /// <summary>
    /// The unique image id
    /// </summary>
    public Guid ImageId { get; set; }

    public int Index { get; set; }

    /// <summary>
    /// Image modality type (Dicom compliant)
    /// </summary>
    public string Modality { get; set; }

    /// <summary>
    /// Image name, given by user
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Original filename that was used when uploading this image
    /// </summary>
    public string? OriginalFilename { get; set; }

    /// <summary>
    /// The origin of this image (Controller / Capture etc..)
    /// </summary>
    public Guid OriginId { get; set; }

    public string SeriesInstanceUid { get; set; } = null!;

    /// <summary>
    /// If several images are related (captured simultaneously)
    /// they share the same SetId
    /// </summary>
    public Guid? SetId { get; set; }

    public string SopInstanceUid { get; set; } = null!;

    /// <summary>
    /// Name of the source
    /// </summary>
    public string? Source { get; set; }

    /// <summary>
    /// Image status
    /// </summary>
    public ImageStatus Status { get; set; }

    /// <summary>
    /// Id of the study this image belongs to
    /// </summary>
    public Guid StudyId { get; set; }

    public int Width { get; set; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; set; }

    public override string ToString()
    {
        return $"Image {ImageId} ({ContentType})";
    }
}
