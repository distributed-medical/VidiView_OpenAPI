namespace VidiView.Api.DataModel.Exceptions;

public class E1004_TimeoutException : VidiViewException
{
    public E1004_TimeoutException(string message)
        : base(message)
    {
    }
}
