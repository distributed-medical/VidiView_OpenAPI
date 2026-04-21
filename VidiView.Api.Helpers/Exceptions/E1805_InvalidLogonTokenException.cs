namespace VidiView.Api.Exceptions;

public class E1805_InvalidLogonTokenException : VidiViewException
{
    public E1805_InvalidLogonTokenException(string message)
        : base(1805, message)
    {
    }
}

