namespace VidiView.Api.DataModel.Exceptions;

public class E1405_ServiceMaintenanceModeException : VidiViewException
{
    public E1405_ServiceMaintenanceModeException(string message, DateTimeOffset expectedOnline)
        : base(message)
    {
        ErrorCode = 1405;
    }
}
