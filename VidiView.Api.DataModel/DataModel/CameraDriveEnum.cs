namespace VidiView.Api.DataModel;

[JsonConverter(typeof(StringEnumConverterEx<CameraDriveEnum>))]
[Flags]
public enum CameraDriveEnum 
{
    None = 0x0,
    PanLeft = 0x1,
    PanRight = 0x2,
    TiltUp = 0x4,
    TiltDown = 0x8,
    ZoomWide = 0x16, // Zoom out
    ZoomTele = 0x32, // Zoom in
}
