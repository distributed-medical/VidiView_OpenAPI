namespace VidiView.Api.Exceptions;

public class E1407_UnsupportedVersionException : VidiViewException
{
    public E1407_UnsupportedVersionException(string message)
        : base(1407, message)
    {
    }
}
