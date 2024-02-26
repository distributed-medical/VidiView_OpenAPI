namespace VidiView.Api.Exceptions;

public class E2123_MaximumDurationExceededException : VidiViewException
{
    public E2123_MaximumDurationExceededException(string message)
        : base(message)
    {
    }
}
