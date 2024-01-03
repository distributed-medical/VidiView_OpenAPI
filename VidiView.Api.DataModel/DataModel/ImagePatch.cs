namespace VidiView.Api.DataModel;
public record ImagePatch
{
    /// <summary>
    /// Image description
    /// </summary>
    public Patch<string?>? Description { get; init; }

    /// <summary>
    /// Image modality type (Dicom compliant)
    /// </summary>
    public Patch<string?>? Modality { get; init; }

    /// <summary>
    /// Image name, given by user
    /// </summary>
    public Patch<string?>? Name { get; init; }

    /// <summary>
    /// Image rotation
    /// </summary>
    public Patch<int>? Rotation { get; init; }

    /// <summary>
    /// Anatomic region
    /// </summary>
    public Patch<string?>? AnatomicRegionXml { get; init; }
}
