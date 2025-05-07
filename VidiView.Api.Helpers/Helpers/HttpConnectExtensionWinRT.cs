#if WINRT
using System.Web;
using System.Runtime.Versioning;
using VidiView.Api.DataModel;
using VidiView.Api.Exceptions;
using VidiView.Api.Headers;
using Windows.Web.Http;

namespace VidiView.Api.Helpers;

[SupportedOSPlatform("windows10.0.17763.0")]
public static class HttpConnectExtensionWinRT
{
    /// <summary>
    /// Try to connect to the specific VidiView Server host. The specified host name may be a full url, host name only or a host and path. 
    /// The implementation will following redirects to get to the correct end point, if possible.
    /// </summary>
    /// <param name="http"></param>
    /// <param name="hostName">Host name. May include optional scheme, port and path</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="UriFormatException"></exception>
    /// <exception cref="E1002_ConnectException"></exception>
    /// <exception cref="E1405_ServiceMaintenanceModeException"></exception>
    /// <exception cref="E1421_NoResponseFromServerException"></exception>
    /// <exception cref="TaskCanceledException"></exception>    /// <returns>
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
        http.Disconnect();
        var state = new ConnectionRequest(hostName);
        return await ConnectAsync(http, state, cancellationToken);
    }

    /// <summary>
    /// Construct a connection helper
    /// </summary>
    /// <param name="hostName">Host name. May include optional scheme, port and path</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="UriFormatException"></exception>
    /// <exception cref="E1002_ConnectException"></exception>
    /// <exception cref="E1405_ServiceMaintenanceModeException"></exception>
    /// <exception cref="E1421_NoResponseFromServerException"></exception>
    /// <exception cref="TaskCanceledException"></exception>
    public static async Task<IConnectState> ConnectAsync(this HttpClient http, IConnectState state, CancellationToken cancellationToken)
    {
        var uri = HttpConnectExtension.HandleState(state, out var callHistory);

        int retryCount = 5;
        while (retryCount-- > 0)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, uri);
            HttpResponseMessage response;
            try
            {
                callHistory.Add(uri);
                response = await http.SendRequestAsync(request).AsTask(cancellationToken);
            }
            catch (Exception ex)
            {
                var inner = NetworkException.CreateFromWinRT(uri, request.TransportInformation.ServerCertificate, ex);
                if (inner is NetworkException ne && ne.Status == Windows.Web.WebErrorStatus.CannotConnect)
                {
                    throw new E1421_NoResponseFromServerException(ne.RequestedUri, ne);
                }
                else
                {                
                    throw new E1002_ConnectException(inner.Message, inner)
                    {
                        Uri = uri
                    };
                }
            }

            switch (response.StatusCode)
            {
                // Follow redirects
                case HttpStatusCode.MovedPermanently:
                case HttpStatusCode.Found:
                case HttpStatusCode.PermanentRedirect:
                case HttpStatusCode.TemporaryRedirect:
                    var redirectUri = response.Headers.Location ?? throw new Exception("No Location header provided for redirect");
                    uri = HttpConnectExtension.HandleRedirect(uri, redirectUri, callHistory, out var preauth);
                    if (preauth != null)
                        return preauth;

                    continue;

                case HttpStatusCode.NotFound:
                    // A json problem here indicates the VidiView Server is answering.
                    await response.AssertNotProblem().ConfigureAwait(false);

                    // Otherwise it is treated as the Web Server responded with the 404
                    uri = HttpConnectExtension.GetDefaultPath(uri);
                    continue;

                default:
                    await response.AssertSuccessAsync();

                    try
                    {
                        // Check if we are successfully connected
                        var home = response.Deserialize<ApiHome>();
                        if (home != null)
                        {
                            // Cache the home uri and api page with this HttpClient instance
                            http.SetAddressAndHome(uri, home);
                            return new ConnectionSuccessful(uri, home, request.TransportInformation.ServerCertificate, callHistory);
                        }
                    }
                    catch
                    {
                    }

                    // Something other than a VidiView Server has answered
                    uri = HttpConnectExtension.GetDefaultPath(uri);
                    continue;
            }

            throw new Exception($"Failed to deserialize ApiHome document");
        }
        while (retryCount-- > 0) ;

        throw new E1002_ConnectException("Too many redirects")
        {
            Uri = callHistory.FirstOrDefault()
        };
    }
}
#endif