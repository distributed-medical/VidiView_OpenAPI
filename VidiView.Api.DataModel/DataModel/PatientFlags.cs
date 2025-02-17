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
}