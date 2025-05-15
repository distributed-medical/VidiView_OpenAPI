namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record PatientId
{
    public Guid? Guid { get; init; }

    /// <summary>
    /// The patient id number (for instance, Swedish personnummer)
    /// </summary>
    public string Number { get; init; } = null!;

    /// <summary>
    /// The assigning authority id 
    /// </summary>
    public string Authority { get; init; } = string.Empty;

    /// <summary>
    /// The desired UI presentation of the patient id
    /// </summary>
    public string Display { get; init; } = string.Empty;

    public override string ToString() => Number ?? "<null>";
}
