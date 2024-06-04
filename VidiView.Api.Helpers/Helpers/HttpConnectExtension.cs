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
    /// Try to connect to the specific VidiView Server host. The specified host name may be a full url, host name only or a host and path. 
    /// The implementation will following redirects to get to the correct end point, if possible.
    /// </summary>
    /// <param name="http"></param>
    /// <param name="hostName">Host name. May include optional scheme, port and path</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// A <see cref="ConnectionSuccessful"/> object when the connection is established. This document will contain server info. The API's uri is cached for the HttpClient instance and used in subsequent calls for the Api home page.
    /// If preauthentication is required, a <see cref="PreauthenticateRequired"/> object is returned with the specific IdP type to use and a redirect uri. When authentication is completed, the <see cref="PreauthenticateRequired"/> instance should be submitted again to the ConnectAsync call to complete the connection
    /// </returns>
    /// <example>
    /// // Connect to host
    ///var connectState = await _http.Client.ConnectAsync(hostName, cancellationToken);
    ///if (connectState is PreauthenticateRequired preauth)
    ///{
    ///    switch (preauth.IdP)
    ///    {
    ///        case PreauthenticateRequired.Oidc:
    ///            await PreAuthenticateUsingOidcAsync(preauth.RedirectUri, cancellationToken);
    ///
    ///            connectState = await _http.Client.ConnectAsync(connectState, cancellationToken);
    ///            break;
    ///
    ///        default:
    ///            throw new NotSupportedException($"The indicated authentication method is not supported by this client: {preauth.IdP ?? "<null>"}");
    ///    }
    ///}
    ///
    ///if (connectState is not ConnectionSuccessful success)
    ///{
    ///    throw new NotSupportedException($"Unhandled connection state returned {connectState?.GetType().Name ?? "<null>"}");
    ///}
    /// </example>
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
