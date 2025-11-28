using VidiView.Api.DataModel;

namespace VidiView.Api.Exceptions;

public class E1740_MediaFileNotPresentException : VidiViewException
{
    public E1740_MediaFileNotPresentException(string message)
        : base(message)
    {
    }

    public RestoreItemRequest? Status { get; init; }
}

