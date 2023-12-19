namespace VidiView.Api.Exceptions;

public class E1405_ServiceMaintenanceModeException : VidiViewException
{
    public DateTimeOffset? ExpectedOnline { get; }
    public E1405_ServiceMaintenanceModeException(string message, DateTimeOffset? expectedOnline)
        : base(message)
    {
        ErrorCode = 1405;
        ExpectedOnline = expectedOnline;
    }
}
