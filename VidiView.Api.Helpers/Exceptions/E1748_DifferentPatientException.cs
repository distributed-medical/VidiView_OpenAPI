namespace VidiView.Api.Exceptions;

public class E1748_DifferentPatientException : VidiViewException
{
    public E1748_DifferentPatientException(string message)
        : base(1748, message)
    {
    }
}
