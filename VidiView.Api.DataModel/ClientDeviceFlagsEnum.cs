namespace VidiView.Api.DataModel;

[Flags]
public enum ClientDeviceFlags : long
{
    None = 0x0,
    SharedDevice = 0x20,
}
