namespace VidiView.Api.DataModel.Exceptions;

public class E1820_ApiKeyRequiredException : VidiViewException
{
    public E1820_ApiKeyRequiredException(string message)
        : base(message)
    {
    }
}
