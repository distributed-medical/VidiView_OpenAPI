namespace VidiView.Api.DataModel.Exceptions;

public class E1032_TooManyRecordsException : VidiViewException
{
    public E1032_TooManyRecordsException(string message)
        : base(message)
    {
    }
}