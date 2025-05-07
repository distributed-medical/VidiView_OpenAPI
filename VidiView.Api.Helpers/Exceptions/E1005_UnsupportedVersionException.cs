namespace VidiView.Api.Exceptions;

public class E1005_UnsupportedVersionException : VidiViewException
{
    public E1005_UnsupportedVersionException(string message)
        : base(message)
    {
        ErrorCode = 1005;
    }
}
