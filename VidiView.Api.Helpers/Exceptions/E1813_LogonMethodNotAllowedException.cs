namespace VidiView.Api.Exceptions;

public class E1813_LogonMethodNotAllowedException : VidiViewException
{
    public E1813_LogonMethodNotAllowedException()
        : base("You are not allowed to use this logon method")
    {
    }
    public E1813_LogonMethodNotAllowedException(string message)
        : base(message)
    {
    }
}
