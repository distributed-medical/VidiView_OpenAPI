using VidiView.Api.DataModel;

namespace VidiView.Api.Configuration.DataModel;

[JsonConverter(typeof(StringEnumConverterEx<SettingFlags>))]
[Flags]
public enum SettingFlags : long
{
    None = 0x0,
    Advanced = 0x1,
    RestartRequired = 0x2,
    UserUpdateable = 0x4,
    Encrypt = 0x8,

    // Possible value overrides
    Server = 0x100,
    Controller = 0x200,
    User = 0x400,
    UserPreference = 0x800,
    Group = 0x1000,
    Role = 0x2000,
    Application = 0x4000,
    Department = 0x10000,
}
