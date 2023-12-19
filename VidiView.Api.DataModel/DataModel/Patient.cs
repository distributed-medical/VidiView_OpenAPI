namespace VidiView.Api.DataModel;

public record Patient 
{
    /// <summary>
    /// The patient ID
    /// </summary>
    public PatientId Id { get; init; } = null!;

    public PersonName Name { get; init; } = null!;

    /// <summary>
    /// Returns true if patient id and/or name has been pseudonymized
    /// </summary>
    public bool Pseudonymized { get; init; }

    public bool ProtectedIdentity { get; init; }

    public string BirthSex { get; init; } = "U";

    public string? GenderIdentity { get; init; }

    public bool Deceased { get; init; }

    public DateTimeOffset? DeceasedDate { get; init; }

    public DateTime? BirthDate { get; init; }

    public int? Age { get; init; }

    [JsonPropertyName("_links")]
    public LinkCollection Links
    {
        get; init;
    }
}
