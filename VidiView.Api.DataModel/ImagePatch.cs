namespace VidiView.Api.DataModel;
public class ImagePatch
{
    /// <summary>
    /// Image description
    /// </summary>
    public PatchString? Description { get; set; }

    /// <summary>
    /// Image modality type (Dicom compliant)
    /// </summary>
    public PatchString? Modality { get; set; }

    /// <summary>
    /// Image name, given by user
    /// </summary>
    public PatchString? Name { get; set; }

    /// <summary>
    /// Image rotation
    /// </summary>
    public PatchInt? Rotation { get; set; }

    /// <summary>
    /// Anatomic region
    /// </summary>
    public PatchString? AnatomicRegionXml { get; set; }
}
