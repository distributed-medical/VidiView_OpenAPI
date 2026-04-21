namespace VidiView.Api.Exceptions;

public class E1718_ReferenceViolationException : VidiViewException
{
    public E1718_ReferenceViolationException(string message)
        : base(1718, message)
    {
    }
}
