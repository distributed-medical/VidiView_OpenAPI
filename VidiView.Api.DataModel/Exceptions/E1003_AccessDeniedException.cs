namespace VidiView.Api.Exceptions;

public class E1003_AccessDeniedException : VidiViewException
{
    public E1003_AccessDeniedException(string message)
        : base(message)
    {
    }
}
