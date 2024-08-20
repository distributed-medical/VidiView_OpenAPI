namespace VidiView.Api.DataModel;

[JsonConverter(typeof(StringEnumConverterEx<ServiceHostType>))]
[Flags]
public enum ServiceHostType
    : long
{
    None = 0,

    // Protocol type
    VidiViewNative = 0x10,
    Dicom = 0x20,
    HL7v2 = 0x40,
    HL7v3 = 0x80,
    HttpRest = 0x100,
    Other = 0x800,

    /// <summary>
    /// Worklist provider
    /// </summary>
    Worklist = 0x1000,

    /// <summary>
    /// PDQ provider
    /// </summary>
    PDQ = 0x2000,

    /// <summary>
    /// Export destination
    /// </summary>
    Export = 0x4000,

    /// <summary>
    /// Import source
    /// </summary>
    Import = 0x8000,

    /// <summary>
    /// IAN destination
    /// </summary>
    IANDestination = 0x10000,

    EnterprisePartner = 0x80000,

    WorklistVidiView = 0x1010,
    WorklistDicom = 0x1020,
    WorklistProvisio = 0x1800,

    PDQProviderHL7v2 = 0x2040,
    PDQProviderHL7v3 = 0x2080,

    ExportVidiView = 0x4010,
    ExportDicom = 0x4020,
    ExportDicomWithQR = 0x4021,
    ExportRMV = 0x4100,

    /// <summary>
    /// Dicom modality receive
    /// </summary>
    ImportDicom = 0x8020,
    ImportDicomQR = 0x8021,

    IANReceiverHL7v2 = 0x10040,
}
