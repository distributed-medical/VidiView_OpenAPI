namespace VidiView.Api.DataModel;

public record PatientSearch
{
    /// <summary>
    /// Specific patient-id
    /// </summary>
    public string? IdNumber { get; init; }

    /// <summary>
    /// Specific id-authority
    /// </summary>
    public string? IdAuthority { get; init; }

    /// <summary>
    /// Search by patient name
    /// </summary>
    /// <remarks>Wildcards supported</remarks>
    public string? Name { get; init; }

    /// <summary>
    /// Search by patient birth date
    /// </summary>
    public DateRange? BirthDate { get; init; }

    /// <summary>
    /// Search for a specific gender
    /// </summary>
    public string? Gender { get; init; }

}
