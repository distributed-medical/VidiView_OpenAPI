namespace VidiView.Api.DataModel;

[JsonConverter(typeof(StringEnumConverterEx<RestoreStatus>))]
public enum RestoreStatus
{
    [Obsolete("This is only used when deserializing unknown values")]
    Unknown = 0,

    /// <summary>
    /// This item is requested, but the request has not yet been responded to
    /// </summary>
    Requested = 1,

    /// <summary>
    /// The request was denied
    /// </summary>
    RequestDenied = 2,

    /// <summary>
    /// An error occurred requesting this item
    /// </summary>
    RequestError = 3,

    /// <summary>
    /// Request has been sent
    /// </summary>
    WaitingForTransfer = 10,

    /// <summary>
    /// The transfer is currently in progress
    /// </summary>
    TransferInProgress = 20,

    /// <summary>
    /// The transfer was aborted
    /// </summary>
    TransferAborted = 21,

    /// <summary>
    /// The transfer failed
    /// </summary>
    TransferFailed = 22,

    /// <summary>
    /// The transfer is completed
    /// </summary>
    TransferCompleted = 30,
}

