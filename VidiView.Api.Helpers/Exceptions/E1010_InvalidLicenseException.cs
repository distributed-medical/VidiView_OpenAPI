namespace VidiView.Api.Exceptions;

public class E1010_InvalidLicenseException : VidiViewException
{
    public E1010_InvalidLicenseException(string message)
        : base(message)
    {
        ErrorCode = 1010;
    }
}
