namespace VidiView.Api.Exceptions;

public class E1406_InvalidUriException : VidiViewException
{
    public E1406_InvalidUriException(Exception innerException)
    : base("The host address entered is not valid", innerException)
    {
        ErrorCode = 1406;
    }

    public E1406_InvalidUriException(string message)
        : base(message)
    {
        ErrorCode = 1406;
    }
}
