namespace VidiView.Api.DataModel;

public enum ExportQueueStatus
{
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
