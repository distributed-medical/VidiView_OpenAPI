namespace VidiView.Api.Exceptions;

public class E1407_UnsupportedVersionException : VidiViewException
{
    public E1407_UnsupportedVersionException(string message)
        : base(message)
    {
        ErrorCode = 1407;
    }
}
