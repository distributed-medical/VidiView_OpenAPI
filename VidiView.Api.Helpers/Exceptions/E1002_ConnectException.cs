namespace VidiView.Api.Exceptions;

[Obsolete("Replaced by E1400_ConnectServerException", true)]
public class E1002_ConnectException : VidiViewException
{
    public E1002_ConnectException(string message, Exception innerException)
        : base(1002, message, innerException)
    {
    }

    public E1002_ConnectException(string message)
        : base(1002, message)
    {
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
