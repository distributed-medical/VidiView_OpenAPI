namespace VidiView.Api.DataModel.Exceptions;

public class E1803_AuthenticationFailedException : VidiViewException
{
    public E1803_AuthenticationFailedException(string message)
        : base(message)
    {
    }
}
