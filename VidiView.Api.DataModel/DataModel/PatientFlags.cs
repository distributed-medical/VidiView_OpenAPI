namespace VidiView.Api.DataModel;

[JsonConverter(typeof(StringEnumConverterEx<PatientFlags>))]
[Flags]
public enum PatientFlags
    : long
{
    None = 0x0,

    /// <summary>
    /// This patient record has been merged into the specified Id.Guid
    /// </summary>
    Merged = 0x0001,

    /// <summary>
    /// The patient record returned is a redirection from the requested id
    /// </summary>
    IsRedirection = 0x0002,
}