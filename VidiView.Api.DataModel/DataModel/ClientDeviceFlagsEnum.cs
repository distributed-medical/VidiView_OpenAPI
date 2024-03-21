namespace VidiView.Api.DataModel;

[JsonConverter(typeof(StringEnumConverterEx<ClientDeviceFlags>))]
[Flags]
public enum ClientDeviceFlags : long
{
    None = 0x0,
    SharedDevice = 0x20,
}
