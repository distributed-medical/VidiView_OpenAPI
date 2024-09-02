namespace VidiView.Api.DataModel;

/// <summary>
/// The Image contains data for a specific image
/// </summary>
public record MediaFile
{
    public DateTimeOffset AcquisitionDate { get; init; }

    /// <summary>
    /// SHA256 checksum of image file. 
    /// Byte array sent as a hex string (2 chars per byte)
    /// </summary>
    public string? Checksum { get; init; }

    public string ContentType { get; init; } = string.Empty;

    /// <summary>
    /// Controller where this image was acquired
    /// </summary>
    public IdAndName? Controller { get; init; }

    /// <summary>
    /// User creating this image
    /// </summary>
    public IdAndName? CreatedBy { get; init; }

    /// <summary>
    /// If this is set, the image is deleted
    /// </summary>
    public DateTimeOffset? DeletedDate { get; init; }

    /// <summary>
    /// Id of image this image was derived from
    /// </summary>
    public IdAndName? DerivedFrom { get; init; }

    public string? Description { get; init; }

    /// <summary>
    /// Id of device where this image was acquired
    /// </summary>
    public Guid? DeviceId { get; init; }

    public TimeSpan? Duration { get; init; }

    public QueuedItemStatus[]? ExportState { get; init; }

    public ImageFlags Flags { get; init; }

    public string Filename { get; init; } = null!;

    public long FileSize { get; init; }

    [Obsolete("This is only used by legacy VidiView Capture iOS?")]
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
    public IdAndName Origin { get; init; }

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
    public MediaFileStatus Status { get; init; }

    /// <summary>
    /// Id of the study this image belongs to
    /// </summary>
    public Guid StudyId { get; init; }

    public int Width { get; init; }

    /// <summary>
    /// True if this media file is marked as a favourite
    /// </summary>
    public bool IsFavourite { get; init; }

    /// <summary>
    /// Image annotations
    /// </summary>
    public Annotation[]? Annotations { get; init; }

    /// <summary>
    /// Anatomic region annotation for this media file
    /// </summary>
    public AnatomicRegion? AnatomicRegion { get; init; }

    /// <summary>
    /// Timestamp value that changes with any update
    /// </summary>
    /// <remarks>
    /// This is only changed when actual file metadata is changed, not
    /// annotations, favourite markings etc
    /// </remarks>
    public string Timestamp { get; init; }

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
