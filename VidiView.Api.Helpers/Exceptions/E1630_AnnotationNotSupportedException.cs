namespace VidiView.Api.Exceptions;

public class E1630_AnnotationNotSupportedException : VidiViewException
{
    public E1630_AnnotationNotSupportedException(string message)
        : base(1630, message)
    {
    }
}
