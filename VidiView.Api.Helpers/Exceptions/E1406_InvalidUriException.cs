namespace VidiView.Api.Exceptions;

public class E1406_InvalidUriException : VidiViewException
{
    public E1406_InvalidUriException(Exception innerException)
    : base(1406, "The host address entered is not valid", innerException)
    {
    }

    public E1406_InvalidUriException(string message)
        : base(1406, message)
    {
    }
}
