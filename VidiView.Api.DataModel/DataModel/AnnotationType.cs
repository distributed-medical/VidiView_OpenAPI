namespace VidiView.Api.DataModel;

[JsonConverter(typeof(StringEnumConverterEx<AnnotationType>))]
public enum AnnotationType
{
    Unknown = 0,
    Rect,
    FilledRect,
    Ellipse,
    FilledEllipse,
    Polygon,
    FilledPolygon,
    Arrow,
    Text,
    Pin,
    Calibration,
    Ruler,
    MeasuredArea,
}
