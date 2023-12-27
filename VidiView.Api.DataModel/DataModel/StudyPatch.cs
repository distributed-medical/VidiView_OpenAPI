namespace VidiView.Api.DataModel;

/// <summary>
/// Update a specific study
/// </summary>
public record StudyPatch
{
    /// <summary>
    /// Study department
    /// </summary>
    public PatchGuid? DepartmentId { get; init; }

    /// <summary>
    /// Study description
    /// </summary>
    public PatchString? Description { get; init; }

    /// <summary>
    /// Study accession number
    /// </summary>
    public PatchString? AccessionNumber { get; init; }

    /// <summary>
    /// Performing physician's name
    /// </summary>
    public PatchString? PerformingPhysician { get; init; }

    /// <summary>
    /// Performing nurse's name
    /// </summary>
    public PatchString? PerformingNurse { get; init; }

    /// <summary>
    /// Assistant 1
    /// </summary>
    public PatchString? Assistant1 { get; init; }

    /// <summary>
    /// Assistant 2
    /// </summary>
    public PatchString? Assistant2 { get; init; }

    /// <summary>
    /// Referring party
    /// </summary>
    public PatchString? ReferringParty { get; init; }

}