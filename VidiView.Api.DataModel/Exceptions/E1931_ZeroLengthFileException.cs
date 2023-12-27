namespace VidiView.Api.Exceptions;

public class E1931_ZeroLengthFileException : VidiViewException
{
    public E1931_ZeroLengthFileException(string message)
        : base(message)
    {
    }
}
