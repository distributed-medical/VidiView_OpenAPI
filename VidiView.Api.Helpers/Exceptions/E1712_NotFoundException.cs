namespace VidiView.Api.Exceptions;

public class E1712_NotFoundException : VidiViewException
{
    public E1712_NotFoundException(string message)
        : base(1712, message)
    {
    }

    protected E1712_NotFoundException(int errorCode, string message)
        : base(errorCode, message)
    {
    }
}
