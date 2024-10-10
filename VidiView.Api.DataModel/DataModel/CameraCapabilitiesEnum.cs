namespace VidiView.Api.DataModel;

[JsonConverter(typeof(StringEnumConverterEx<CameraCapabilities>))]
[Flags]
public enum CameraCapabilities
{
    None = 0x0,
    Pan = 0x1,
    Tilt = 0x2,
    Zoom = 0x4,
    Rotate = 0x10,
}
