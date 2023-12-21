namespace VidiView.Configuration.Api;

public record DicomConnection
{
    public string Host { get; init; }
    public int Port { get; init; }
    public bool RequireTLS { get; init; }
    public string CallingAeTitle { get; init; }
    public string RemoteAeTitle { get; init; }

    public int MaxReceivePduLength { get; init; } = 65536;
    public TimeSpan ConnectionTimeout { get; init; } = TimeSpan.FromSeconds(20);

    public TimeSpan CommandTimeout { get; init; } = TimeSpan.FromMinutes(30);

    public int MaxConnections { get; init; } = 4;
}
