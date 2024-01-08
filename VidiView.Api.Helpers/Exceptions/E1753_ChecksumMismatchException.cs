namespace VidiView.Api.Exceptions;

public class E1753_ChecksumMismatchException : VidiViewException
{
    public E1753_ChecksumMismatchException(string message)
        : base(message)
    {
    }
}
