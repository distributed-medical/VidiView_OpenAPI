namespace VidiView.Configuration.Api;

[Flags]
public enum ServiceHostType
    : long
{
    /// <summary>
    /// External Dicom modality from which images are received
    /// </summary>
    DicomModalityReceive = 0x201,

    // External archives
    //        ExternalArchive =                           0x400,
    //        ExternalArchive_Export =                    0x401,
    
    /// <summary>
    /// External Dicom compatible archive used for query/retrieve
    /// </summary>
    RetrieveDicom = 0x402,
    
    /// <summary>
    /// External Dicom compatible export destination
    /// </summary>
    ExportDicom = 0x411,
//        ExternalArchive_DicomCStoreQueryRetrieve = 0x413, // ExternalArchive_DicomCStore | ExternalArchive_DicomQueryRetrieve,
    
    /// <summary>
    /// External VidiView archive export destination
    /// </summary>
    ExportVidiView = 0x421,

    EnterprisePartner = 0x800,

    WorklistProvider = 0x1000,
    WorklistProvider_None = 0x1000,
    WorklistProvider_VidiView = 0x1001,
    WorklistProvider_Dicom = 0x1002,
    WorklistProvider_Provisio = 0x1004,

    HL7v2_Host = 0x2000,
    HL7v3_Host = 0x4000,

    PDQ_Provider_HL7v2 = 0x12000,
    PDQ_Provider_HL7v3 = 0x14000,

    IAN_Receiver_HL7v2 = 0x22000,
}
