namespace VidiView.Api.Exceptions;

public class E1002_ConnectException : VidiViewException
{
    public E1002_ConnectException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public E1002_ConnectException(string message)
        : base(message)
    {
    }

    public Uri? Uri { get; init; }
}
