namespace VidiView.Api.Exceptions;

public class E1311_InsufficientDiskSpaceException : VidiViewException
{
    public E1311_InsufficientDiskSpaceException(string message)
        : base(message)
    {
        ErrorCode = 1311;
    }
}
