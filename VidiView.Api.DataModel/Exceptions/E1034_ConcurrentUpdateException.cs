namespace VidiView.Api.DataModel.Exceptions;

public class E1034_ConcurrentUpdateException : VidiViewException
{
    public E1034_ConcurrentUpdateException(string message)
        : base(message)
    {
    }
}
