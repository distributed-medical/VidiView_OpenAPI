namespace VidiView.Api.DataModel;

[JsonConverter(typeof(StringEnumConverterEx<ExportQueueStatus>))]
public enum ExportQueueStatus
{
    Unknown = 0,

    Tentative = 1,
    ReadyForTransfer = 20,
    TransferInProgress = 30,
    TransferFailed = 60,
    TransferCompleted = 70
}
