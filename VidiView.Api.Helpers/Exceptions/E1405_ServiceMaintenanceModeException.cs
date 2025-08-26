namespace VidiView.Api.Exceptions;

public class E1405_ServiceMaintenanceModeException : E1400_ConnectServerException
{
    public E1405_ServiceMaintenanceModeException(Uri requestedUri, string message, DateTimeOffset? expectedOnline)
        : base(message)
    {
        ErrorCode = 1405;
        ExpectedOnline = expectedOnline;
        RequestedUri = requestedUri;
    }

    public DateTimeOffset? ExpectedOnline { get; init; }
}
