namespace VidiView.Api.DataModel;

[JsonConverter(typeof(StringEnumConverterEx<TypeFlags>))]
[Flags]
public enum TypeFlags
    : long
{
    None = 0x0,
    SystemDefined = 0x1,
    IsActive = 0x2,

    AcceptAnyMediaType = 0x100,
    RequireMediaType = 0x200
}