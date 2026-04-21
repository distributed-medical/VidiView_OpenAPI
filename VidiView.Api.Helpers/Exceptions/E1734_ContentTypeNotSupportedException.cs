namespace VidiView.Api.Exceptions;

public class E1734_ContentTypeNotSupportedException : VidiViewException
{
    public E1734_ContentTypeNotSupportedException(string message)
        : base(1734, message)
    {
    }

    public string? ContentType { get; init; }
}
