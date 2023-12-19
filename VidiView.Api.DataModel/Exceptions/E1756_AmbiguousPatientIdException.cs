namespace VidiView.Api.Exceptions;

public class E1756_AmbiguousPatientIdException : VidiViewException
{
    public E1756_AmbiguousPatientIdException(string message)
        : base(message)
    {
    }
}
