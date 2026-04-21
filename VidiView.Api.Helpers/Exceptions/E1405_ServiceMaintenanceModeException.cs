namespace VidiView.Api.Exceptions;

public class E1405_ServiceMaintenanceModeException : E1400_ConnectServerException
{
    public E1405_ServiceMaintenanceModeException(Uri requestedUri, string message, DateTimeOffset? expectedOnline)
        : base(1405, message)
    {
        ExpectedOnline = expectedOnline;
        RequestedUri = requestedUri;
    }

    public DateTimeOffset? ExpectedOnline { get; init; }
}
