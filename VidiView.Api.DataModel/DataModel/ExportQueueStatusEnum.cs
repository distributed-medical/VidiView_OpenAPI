namespace VidiView.Api.DataModel;

[JsonConverter(typeof(StringEnumConverterEx<ExportQueueStatus>))]
public enum ExportQueueStatus
{
    Unknown = 0,

    // < 30 = Prepare phase
    AddedToQueue = 0,
    ConversionInProgress = 10,
    ReadyForTransfer = 20,

    // >= 30 Progress phase
    TransferInProgress = 30,

    // >= 50 Result phase
    ConversionFailed = 50,
    TransferFailed = 60,
    TransferCompleted = 70
}
