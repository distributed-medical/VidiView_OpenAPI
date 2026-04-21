namespace VidiView.Api.Exceptions;

public class E1030_NotSupportedException : VidiViewException
{
    public E1030_NotSupportedException(string message)
        : base(1030, message)
    {
    }
}
