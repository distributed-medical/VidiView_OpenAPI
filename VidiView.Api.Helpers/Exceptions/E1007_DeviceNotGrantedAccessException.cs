namespace VidiView.Api.Exceptions;

public class E1007_DeviceNotGrantedAccessException : VidiViewException
{
    public E1007_DeviceNotGrantedAccessException(string message)
        : base(message)
    {
    }
}