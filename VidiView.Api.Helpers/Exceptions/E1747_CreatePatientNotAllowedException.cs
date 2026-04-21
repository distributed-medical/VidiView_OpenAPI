namespace VidiView.Api.Exceptions;

public class E1747_CreatePatientNotAllowedException : VidiViewException
{
    public E1747_CreatePatientNotAllowedException(string message)
        : base(1747, message)
    {
    }
}
