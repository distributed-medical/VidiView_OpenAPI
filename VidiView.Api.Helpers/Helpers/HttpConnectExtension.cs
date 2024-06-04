using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using VidiView.Api.DataModel;

namespace VidiView.Api.Helpers;

public static class HttpConnectExtension
{
    const string DefaultPath = "/vidiview/api/";

    /// <summary>
    /// Try to connect to the specific VidiView Server host
    /// </summary>
    /// <param name="http"></param>
    /// <param name="hostName">Host name. May include optional scheme, port and path</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<IConnectState> ConnectAsync(this HttpClient http, string hostName, CancellationToken cancellationToken)
    {
        var state = new ConnectionRequest(hostName);
        return await ConnectAsync(http, state, cancellationToken);
    }

    /// <summary>
    /// Construct a connection helper
    /// </summary>
    /// <param name="hostName">Host name. May include optional scheme, port and path</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="UriFormatException"></exception>
    public static async Task<IConnectState> ConnectAsync(this HttpClient http, IConnectState state, CancellationToken cancellationToken)
    {
        List<Uri> redirectHistory;
        Uri uri;
        int followRedirectCount = 3;

        switch (state)
        {
            case ConnectionRequest connectionRequest:
                // This is the initial request
                uri = connectionRequest.ApiUri;
                redirectHistory = new();
                break;

            case PreauthenticateRequired idp:
                // IdP authentication completed successfully
                uri = idp.ApiUri; // The api
                redirectHistory = idp.RedirectHistory;
                break;

            case ConnectionSuccessful:
                throw new InvalidOperationException("Connection already completed");

            default:
                throw new NotImplementedException();
        }

        do
        {
            var response = await http.GetAsync(uri, cancellationToken).ConfigureAwait(false);

            switch (response.StatusCode)
            {
                // Follow redirects
                case HttpStatusCode.MovedPermanently:
                case HttpStatusCode.Found:
                case HttpStatusCode.PermanentRedirect:
                case HttpStatusCode.TemporaryRedirect:
                    redirectHistory.Add(uri); // The called URI was redirected

                    var redirectUri = response.Headers.Location ?? throw new Exception("No Location header provided for redirect");
                    var queryParameters = HttpUtility.ParseQueryString(redirectUri.Query);
                    var type = queryParameters["idp"];
                    if (!string.IsNullOrEmpty(type))
                    {
                        // Redirect to external Idp
                        return new PreauthenticateRequired(type, redirectUri, uri, redirectHistory);
                    }

                    // Just a redirect. Follow and see what happens
                    if (--followRedirectCount < 0)
                        throw new Exception("Too many redirects");

                    uri = redirectUri;
                    continue;

                case HttpStatusCode.NotFound:
                    if (uri.AbsolutePath == DefaultPath)
                        goto default;

                    // Retry using our default path
                    redirectHistory.Add(uri); // The called URI was redirected
                    uri = new Uri(uri.Scheme + "://" + uri.Host + DefaultPath);
                    continue;

                default:
                    await response.AssertSuccessAsync();
                    var home = response.Deserialize<ApiHome>();
                    if (home != null)
                    {
                        // Connection successful
                        http.SetAddressAndHome(uri, home);
                        return new ConnectionSuccessful(uri, home, redirectHistory);
                    }

                    break;
            }

            throw new Exception($"Failed to deserialize ApiHome document");
        }
        while (true);
    }
}
