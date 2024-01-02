namespace VidiView.Api.Exceptions;

public class E1034_ConcurrentUpdateException : VidiViewException
{
    public object? UpdatedInstance { get; set; }

    public E1034_ConcurrentUpdateException(string message)
        : base(message)
    {
    }
}
