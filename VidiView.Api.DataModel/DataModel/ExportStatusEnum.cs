namespace VidiView.Api.DataModel;

[JsonConverter(typeof(StringEnumConverterEx<ExportStatus>))]
public enum ExportStatus
{
    [Obsolete("This is only used when deserializing unknown values")]
    Unknown = 0,

    /// <summary>
    /// This item is placed in the queue, but is not confirmed to be transferred
    /// </summary>
    Tentative = 1,

    /// <summary>
    /// Converting item to destination format
    /// </summary>
    Conversion = 10,

    /// <summary>
    /// This item is ready to be transferred
    /// </summary>
    ReadyForTransfer = 20,

    // < 30 = Prepare phase
    // >= 30 Progress phase
    TransferInProgress = 30,

    // >= 50 Result phase
    ConversionFailed = 50,

    TransferFailed = 60,
    TransferCompleted = 70,
}
