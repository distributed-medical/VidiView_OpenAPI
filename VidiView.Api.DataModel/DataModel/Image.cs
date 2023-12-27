namespace VidiView.Api.DataModel;

/// <summary>
/// The Image contains data for a specific image
/// </summary>
public record Image
{
    public DateTimeOffset AcquisitionDate { get; init; }

    /// <summary>
    /// SHA256 checksum of image file. 
    /// Byte array sent as a hex string (2 chars per byte)
    /// </summary>
    public string? Checksum { get; init; }

    public string ContentType { get; init; } = null!;

    /// <summary>
    /// Id of Controller where this image was acquired
    /// </summary>
    public Guid? ControllerId { get; init; }

    /// <summary>
    /// Id of user creating this image
    /// </summary>
    public Guid? CreatedBy { get; init; }

    /// <summary>
    /// If this is set, the image is deleted
    /// </summary>
    public DateTimeOffset? DeletedDate { get; init; }

    /// <summary>
    /// Id of image this image was derived from
    /// </summary>
    public Guid? DerivedFrom { get; init; }

    public string? Description { get; init; }

    /// <summary>
    /// Id of device where this image was acquired
    /// </summary>
    public Guid? DeviceId { get; init; }

    public TimeSpan? Duration { get; init; }

    public ImageFlags Flags { get; init; }

    public string Filename { get; init; } = null!;

    public long FileSize { get; init; }

    [Obsolete("This is only used by iOS?")]
    public string? FileSizeAsString { get; init; }

    public int Height { get; init; }

    /// <summary>
    /// The unique image id
    /// </summary>
    public Guid ImageId { get; init; }

    public int Index { get; init; }

    /// <summary>
    /// Image modality type (Dicom compliant)
    /// </summary>
    public string? Modality { get; init; }

    /// <summary>
    /// Image name, given by user
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Original filename of a file uploaded to the server
    /// </summary>
    public string? OriginalFilename { get; init; }

    /// <summary>
    /// The origin of this image (Controller / Capture etc..)
    /// </summary>
    public Guid OriginId { get; init; }

    /// <summary>
    /// Image rotation. If null - check for Exif tags to determine rotation
    /// </summary>
    public int? Rotation { get; init; }

    public string SeriesInstanceUid { get; init; } = null!;

    /// <summary>
    /// If several images are related (captured simultaneously)
    /// they share the same SetId
    /// </summary>
    public Guid? SetId { get; init; }

    public string SopInstanceUid { get; init; } = null!;

    /// <summary>
    /// Name of the source
    /// </summary>
    public string? Source { get; init; }

    public string? StationName { get; init; }

    /// <summary>
    /// Image status
    /// </summary>
    public ImageStatus Status { get; init; }

    /// <summary>
    /// Id of the study this image belongs to
    /// </summary>
    public Guid StudyId { get; init; }

    public int Width { get; init; }

    /// <summary>
    /// Any image annotations
    /// </summary>
    public Annotation[]? Annotations { get; init; }

    public AnatomicRegion? AnatomicRegion { get; init; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }

    public override string ToString()
    {
        return $"Image {ImageId} ({ContentType})";
    }
}
