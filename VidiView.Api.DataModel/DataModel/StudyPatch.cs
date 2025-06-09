namespace VidiView.Api.DataModel;

/// <summary>
/// Update a specific study
/// </summary>
[ExcludeFromCodeCoverage]
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
    public Patch<string?>? PerformingPhysicianName { get; init; }

    /// <summary>
    /// Performing nurse's name
    /// </summary>
    public Patch<string?>? PerformingNurseName { get; init; }

    /// <summary>
    /// Assistant 1
    /// </summary>
    public Patch<string?>? Assistant1Name { get; init; }

    /// <summary>
    /// Assistant 2
    /// </summary>
    public Patch<string?>? Assistant2Name { get; init; }

    /// <summary>
    /// Referring physician's name
    /// </summary>
    public Patch<string?>? ReferringPhysicianName { get; init; }

    /// <summary>
    /// Study type
    /// </summary>
    public Patch<Guid>? StudyTypeId { get; init; }

}