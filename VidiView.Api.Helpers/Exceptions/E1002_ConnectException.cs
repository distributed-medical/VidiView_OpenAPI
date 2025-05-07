namespace VidiView.Api.Exceptions;

public class E1002_ConnectException : VidiViewException
{
    public E1002_ConnectException(string message, Exception innerException)
        : base(message, innerException)
    {
        ErrorCode = 1002;
    }

    public E1002_ConnectException(string message)
        : base(message)
    {
        ErrorCode = 1002;
    }

    /// <summary>
    /// The requested Uri
    /// </summary>
    public Uri? Uri { get; init; }

    /// <summary>
    /// The host is responding, but does not appear to be a VidiView Server
    /// </summary>
    public bool NotVidiViewServer { get; init; }
}
