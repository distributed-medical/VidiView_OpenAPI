namespace VidiView.Api.Exceptions;

public class E1931_ZeroLengthFileException : VidiViewException
{
    public E1931_ZeroLengthFileException(string message)
        : base(1931, message)
    {
    }

    public E1931_ZeroLengthFileException(string message, string? fileName)
        : base(1931, message)
    {
        FileName = fileName;
    }

    public string? FileName { get; init; }
}
