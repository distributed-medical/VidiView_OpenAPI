namespace VidiView.Api.DataModel;

[JsonConverter(typeof(StringEnumConverterEx<StudyFlags>))]
[Flags]
public enum StudyFlags
    : long
{
    None = 0,

    /// <summary>
    /// This study is locked
    /// </summary>
    IsLocked = 0x0001,


}
