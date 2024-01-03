namespace VidiView.Api.DataModel;

/// <summary>
/// Update a specific study
/// </summary>
public record StudyPatch
{
    /// <summary>
    /// Study department
    /// </summary>
    public Patch<Guid>? DepartmentId { get; init; }

    /// <summary>
    /// Study description
    /// </summary>
    public Patch<string?>? Description { get; init; }

    /// <summary>
    /// Study accession number
    /// </summary>
    public Patch<string?>? AccessionNumber { get; init; }

    /// <summary>
    /// Performing physician's name
    /// </summary>
    public Patch<string?>? PerformingPhysician { get; init; }

    /// <summary>
    /// Performing nurse's name
    /// </summary>
    public Patch<string?>? PerformingNurse { get; init; }

    /// <summary>
    /// Assistant 1
    /// </summary>
    public Patch<string?>? Assistant1 { get; init; }

    /// <summary>
    /// Assistant 2
    /// </summary>
    public Patch<string?>? Assistant2 { get; init; }

    /// <summary>
    /// Referring party
    /// </summary>
    public Patch<string?>? ReferringParty { get; init; }

}