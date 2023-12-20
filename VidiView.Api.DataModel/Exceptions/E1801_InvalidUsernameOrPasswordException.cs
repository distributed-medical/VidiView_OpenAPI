namespace VidiView.Api.DataModel.Exceptions;

public class E1801_InvalidUsernameOrPasswordException : VidiViewException
{
    public E1801_InvalidUsernameOrPasswordException(string message)
        : base(message)
    {
    }
}
