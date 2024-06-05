using VidiView.Api.DataModel;
#if WINRT
using Windows.Security.Cryptography.Certificates;
#endif

namespace VidiView.Api.Helpers;

/// <summary>
/// Returned if the connection completed successfully
/// </summary>
public class ConnectionSuccessful : IConnectState
{
    public ConnectionSuccessful(Uri uri, ApiHome home, List<Uri> callHistory)
    {
        ApiHome = home;
        Uri = uri;
        CallHistory = callHistory;
    }

#if WINRT
    public ConnectionSuccessful(Uri apiUri, ApiHome home, Certificate? certificate, List<Uri> callHistory)
    {
        ApiHome = home;
        Uri = apiUri;
        CallHistory = callHistory;
        Certificate = certificate;
    }

    /// <summary>
    /// The server host's certificate
    /// </summary>
    public Certificate? Certificate { get; }
#endif

    /// <summary>
    /// The Uri to the Api
    /// </summary>
    public Uri Uri { get; }

    /// <summary>
    /// The requested host name
    /// </summary>
    public ApiHome ApiHome { get; }

    /// <summary>
    /// Uris processed to reach this result
    /// </summary>
    public List<Uri> CallHistory { get; }
}
