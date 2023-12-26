namespace VidiView.Api.DataModel;

/// <summary>
/// Update a specific study
/// </summary>
public class StudyPatch
{
    /// <summary>
    /// Study department
    /// </summary>
    public PatchGuid? DepartmentId { get; set; }

    /// <summary>
    /// Study description
    /// </summary>
    public PatchString? Description { get; set; }

    /// <summary>
    /// Study accession number
    /// </summary>
    public PatchString? AccessionNumber { get; set; }

    /// <summary>
    /// Performing physician's name
    /// </summary>
    public PatchString? PerformingPhysician { get; set; }

    /// <summary>
    /// Performing nurse's name
    /// </summary>
    public PatchString? PerformingNurse { get; set; }

    /// <summary>
    /// Assistant 1
    /// </summary>
    public PatchString? Assistant1 { get; set; }

    /// <summary>
    /// Assistant 2
    /// </summary>
    public PatchString? Assistant2 { get; set; }

    /// <summary>
    /// Referring party
    /// </summary>
    public PatchString? ReferringParty { get; set; }

}