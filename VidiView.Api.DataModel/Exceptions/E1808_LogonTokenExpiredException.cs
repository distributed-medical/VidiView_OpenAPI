namespace VidiView.Api.DataModel.Exceptions;

public class E1808_LogonTokenExpiredException : VidiViewException
{
    public E1808_LogonTokenExpiredException(string message)
        : base(message)
    {
    }
}
