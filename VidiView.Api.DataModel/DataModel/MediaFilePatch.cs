namespace VidiView.Api.DataModel;
public record MediaFilePatch
{
    /// <summary>
    /// File description
    /// </summary>
    public Patch<string?>? Description { get; init; }

    /// <summary>
    /// File modality type (Dicom compliant)
    /// </summary>
    public Patch<string?>? Modality { get; init; }

    /// <summary>
    /// File name, given by user
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
