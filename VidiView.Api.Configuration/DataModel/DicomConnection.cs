namespace VidiView.Configuration.Api;

public class DicomConnection
{
    public string Host { get; set; }
    public int Port { get; set; }
    public bool RequireTLS { get; set; }
    public string CallingAeTitle { get; set; }
    public string RemoteAeTitle { get; set; }
    public int MaxReceivePduLength { get; set; } = 65536;
    public TimeSpan ConnectionTimeout { get; set; } = TimeSpan.FromSeconds(20);

    public TimeSpan CommandTimeout { get; set; } = TimeSpan.FromMinutes(30);

    public int MaxConnections { get; set; } = 4;
}
