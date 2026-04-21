namespace VidiView.Api.Exceptions;

public class E1008_ScopeNotGrantedException : VidiViewException
{
    public E1008_ScopeNotGrantedException(string message)
        : base(1008, message)
    {
    }
}