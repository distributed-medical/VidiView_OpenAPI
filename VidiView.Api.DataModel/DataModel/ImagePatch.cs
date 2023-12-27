namespace VidiView.Api.DataModel;
public record ImagePatch
{
    /// <summary>
    /// Image description
    /// </summary>
    public PatchString? Description { get; init; }

    /// <summary>
    /// Image modality type (Dicom compliant)
    /// </summary>
    public PatchString? Modality { get; init; }

    /// <summary>
    /// Image name, given by user
    /// </summary>
    public PatchString? Name { get; init; }

    /// <summary>
    /// Image rotation
    /// </summary>
    public PatchInt? Rotation { get; init; }

    /// <summary>
    /// Anatomic region
    /// </summary>
    public PatchString? AnatomicRegionXml { get; init; }
}
