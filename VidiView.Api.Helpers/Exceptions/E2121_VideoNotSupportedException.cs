namespace VidiView.Api.Exceptions;

public class E2121_VideoNotSupportedException : VidiViewException
{
    public E2121_VideoNotSupportedException(string message)
        : base(message)
    {
    }
}
