namespace VidiView.Api.Helpers;

public class ConnectionRequest : IConnectState
{
    const string DefaultScheme = "https://";

    /// <summary>
    /// Create a ConnectionRequest
    /// </summary>
    /// <param name="hostName">Host name. May include optional scheme, port and path</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="UriFormatException"></exception>
    public ConnectionRequest(string hostName)
    {
        ArgumentNullException.ThrowIfNull(hostName, nameof(hostName));
        if (string.IsNullOrWhiteSpace(hostName))
            throw new ArgumentException("Blank host name not supported", nameof(hostName));

        HostName = hostName;
        RequestUri = ConvertToUri(hostName);
    }

    /// <summary>
    /// The requested host name
    /// </summary>
    public string HostName { get; }

    /// <summary>
    /// The Uri to the Api
    /// </summary>
    public Uri RequestUri { get; }

    static Uri ConvertToUri(string hostName)
    {
        var success = Uri.TryCreate(hostName, UriKind.RelativeOrAbsolute, out var uri);
        if (!success)
            throw new UriFormatException("Supplied hostName can not be converted to a valid Uri");

        if (!uri!.IsAbsoluteUri)
            uri = new Uri(DefaultScheme + hostName);
        else if (string.IsNullOrEmpty(uri.Host) && hostName.IndexOf("://") == -1)
            uri = new Uri(DefaultScheme + hostName);

        return uri;
    }
}
