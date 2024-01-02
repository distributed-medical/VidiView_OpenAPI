namespace VidiView.Api.DataModel;

public record Patient 
{
    /// <summary>
    /// The patient ID
    /// </summary>
    public PatientId Id { get; init; } = null!;

    /// <summary>
    /// The person name 
    /// </summary>
    public PersonName Name { get; init; } = null!;

    /// <summary>
    /// Returns true if patient id and/or name has been pseudonymized
    /// </summary>
    public bool Pseudonymized { get; init; }

    /// <summary>
    /// True if this person has a protected identity
    /// </summary>
    public bool ProtectedIdentity { get; init; }

    /// <summary>
    /// Birth sex (M/F/U)
    /// </summary>
    public string BirthSex { get; init; } = "U";

    /// <summary>
    /// Gender identity string
    /// </summary>
    public string? GenderIdentity { get; init; }

    /// <summary>
    /// Birth date
    /// </summary>
    public DateTime? BirthDate { get; init; }

    /// <summary>
    /// Age. Calculated from birth date
    /// </summary>
    public int? Age { get; init; }

    /// <summary>
    /// Deceased state
    /// </summary>
    public bool Deceased { get; init; }

    /// <summary>
    /// Deceased date, if known
    /// </summary>
    public DateTimeOffset? DeceasedDate { get; init; }

    /// <summary>
    /// The last study date for this patient
    /// </summary>
    public DateTimeOffset? LastStudyDate { get; init; }

    /// <summary>
    /// When performing a patient lookup, this property may contain
    /// additional information that is vital for creating a study.
    /// </summary>
    public string? SopInstance { get; init; }

    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }
}
