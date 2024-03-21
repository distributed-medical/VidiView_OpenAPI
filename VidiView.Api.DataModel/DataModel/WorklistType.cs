namespace VidiView.Api.DataModel;


[JsonConverter(typeof(StringEnumConverterEx<WorklistType>))]
public enum WorklistType
{
    None = 0,
    DicomWorklist,
    DicomWorklistSupportingAdHoc,
    VidiView,
    Provisio,
}
