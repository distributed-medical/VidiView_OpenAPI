namespace VidiView.Api.Exceptions;

public class E1731_UnidentifiedStudyException : VidiViewException
{
    public E1731_UnidentifiedStudyException(string message)
        : base(1731, message)
    {
    }
}

