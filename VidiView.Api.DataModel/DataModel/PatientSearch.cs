namespace VidiView.Api.DataModel;

public class PatientSearch
{
    /// <summary>
    /// Specific patient-id
    /// </summary>
    public string? IdNumber { get; set; }

    /// <summary>
    /// Specific id-authority
    /// </summary>
    public string? IdAuthority { get; set; }

    /// <summary>
    /// Search by patient name
    /// </summary>
    /// <remarks>Wildcards supported</remarks>
    public string? Name { get; set; }

    /// <summary>
    /// Search by patient birth date
    /// </summary>
    public DateRange? BirthDate { get; set; }

    /// <summary>
    /// Search for a specific gender
    /// </summary>
    public string? Gender { get; set; }

}
