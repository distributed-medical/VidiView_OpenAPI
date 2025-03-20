namespace VidiView.Api.DataModel;

public record ServiceHostDicomConnection
{
    /// <summary>
    /// The service host name
    /// </summary>
    public string Host { get; init; }

    /// <summary>
    /// The service host port
    /// </summary>
    public int Port { get; init; }

    /// <summary>
    /// Require Tls security
    /// </summary>
    public bool RequireTls { get; init; }

    /// <summary>
    /// Calling AE title
    /// </summary>
    public string CallingAeTitle { get; init; }

    /// <summary>
    /// Remote AE title
    /// </summary>
    public string RemoteAeTitle { get; init; }

    /// <summary>
    /// Maximum number of concurrent connections to this host
    /// </summary>
    public int ConcurrentConnections { get; init; }

    /// <summary>
    /// The max PDU (protocol data units) size 
    /// </summary>
    public int MaxPduLength { get; init; }

    /// <summary>
    /// Time to wait when establishing a connection to host
    /// </summary>
    public TimeSpan ConnectionTimeout { get; init; }

    /// <summary>
    /// Maximum time allowed for a command to execute
    /// </summary>
    public TimeSpan CommandTimeout { get; init; }

    /// <summary>
    /// Time between retries
    /// </summary>
    public TimeSpan RetryInterval { get; init; }
}
