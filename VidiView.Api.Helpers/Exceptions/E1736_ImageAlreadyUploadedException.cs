namespace VidiView.Api.Exceptions;

public class E1736_ImageAlreadyUploadedException : VidiViewException
{
    public E1736_ImageAlreadyUploadedException(string message)
        : base(1736, message)
    {
    }
}
