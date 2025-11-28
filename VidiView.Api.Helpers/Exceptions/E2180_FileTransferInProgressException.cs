using VidiView.Api.DataModel;

namespace VidiView.Api.Exceptions;

public class E2180_FileTransferInProgressException : VidiViewException
{
    public E2180_FileTransferInProgressException(string message)
        : base(message)
    {
    }

    public RestoreItemRequest? Status { get; init; }
}

