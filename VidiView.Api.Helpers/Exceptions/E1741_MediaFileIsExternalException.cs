using VidiView.Api.DataModel;

namespace VidiView.Api.Exceptions;

public class E1741_MediaFileIsExternalException : VidiViewException
{
    public E1741_MediaFileIsExternalException(string message)
        : base(message)
    {
    }
}

