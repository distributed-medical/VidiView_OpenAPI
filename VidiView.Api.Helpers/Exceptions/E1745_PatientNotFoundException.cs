namespace VidiView.Api.Exceptions;

public class E1745_PatientNotFoundException : E1712_NotFoundException
{
    public E1745_PatientNotFoundException(string message)
        : base(1745, message)
    {
    }
}
