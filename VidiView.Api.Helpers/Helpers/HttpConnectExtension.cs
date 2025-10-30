using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Web;
using VidiView.Api.DataModel;
using VidiView.Api.Exceptions;
using static System.Net.WebRequestMethods;

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
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="UriFormatException"></exception>
    /// <exception cref="E1002_ConnectException"></exception>
    /// <exception cref="E1405_ServiceMaintenanceModeException"></exception>
    /// <exception cref="E1401_NoResponseFromServerException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
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
    /// <exception cref="E1401_NoResponseFromServerException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    public static async Task<IConnectState> ConnectAsync(this HttpClient http, IConnectState state, CancellationToken cancellationToken)
    {
        var uri = HandleState(state, out var callHistory);

        try
        {
            int retryCount = 5;
            while (retryCount-- > 0)
            {
                HttpResponseMessage response;
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    callHistory.Add(uri);
                    response = await http.GetAsync(uri, cancellationToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    throw new E1401_NoResponseFromServerException(uri);
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    if (ex.TryGetInnerException<AuthenticationException>(out var ae))
                    {
                        // This is the best exception to find detailed info
                        message = ae.Message;
                    }
                    else if (ex.TryGetInnerException<System.Net.Sockets.SocketException>(out var se))
                    {
                        if (se.SocketErrorCode == System.Net.Sockets.SocketError.TimedOut)
                            throw new E1401_NoResponseFromServerException(uri, ex);

                        // This is the second best exception to find detailed info
                        message = se.Message;
                    }

                    throw new E1400_ConnectServerException(message, ex)
                    {
                        RequestedUri = uri
                    };
                }

                switch (response.StatusCode)
                {
                    // Follow redirects
                    case HttpStatusCode.MovedPermanently:
                    case HttpStatusCode.Found:
                    case HttpStatusCode.PermanentRedirect:
                    case HttpStatusCode.TemporaryRedirect:
                        var redirectUri = response.Headers.Location ?? throw new Exception("No Location header provided for redirect");
                        uri = HandleRedirect(uri, redirectUri, callHistory, out var preauth);
                        if (preauth != null)
                            return preauth;

                        continue;

                    case HttpStatusCode.ServiceUnavailable:
                    case HttpStatusCode.NotFound:
                        // A json problem here indicates the VidiView Server is answering.
                        await response.AssertNotProblemAsync().ConfigureAwait(false);
                        await response.AssertNotMaintenanceModeAsync(http).ConfigureAwait(false);

                        // Otherwise it is treated as the Web Server responded with the 404
                        uri = GetDefaultPath(uri);
                        continue;

                    default:
                        try
                        {
                            await response.AssertSuccessAsync().ConfigureAwait(false);
                        }
                        catch (E1405_ServiceMaintenanceModeException)
                        {
                            throw;
                        }
                        catch
                        {
                            try
                            {
                                uri = GetDefaultPath(uri);
                                continue;
                            }
                            catch
                            {
                            }

                            // Throw first exception here
                            throw;
                        }

                        try
                        {
                            // Check if we are successfully connected
                            var home = response.Deserialize<ApiHome>();
                            if (home != null)
                            {
                                // Cache the home uri and api page with this HttpClient instance
                                http.SetAddressAndHome(uri, home);
                                return new ConnectionSuccessful(uri, home, callHistory);
                            }
                        }
                        catch
                        {
                        }

                        // Something other than a VidiView Server has answered
                        uri = GetDefaultPath(uri);
                        continue;
                }
            }

            throw new E1400_ConnectServerException("Too many redirects")
            {
                RequestedUri = callHistory.FirstOrDefault()
            };
        }
        catch
        {
            // Throw cancellation if the token is cancelled
            cancellationToken.ThrowIfCancellationRequested();

            throw;
        }
    }

    internal static Uri HandleState(IConnectState state, out List<Uri> callHistory)
    {
        switch (state)
        {
            case ConnectionRequest connectionRequest:
                // This is the initial request
                callHistory = new();
                return connectionRequest.RequestUri;

            case PreauthenticateRequired idp:
                // IdP authentication completed successfully
                callHistory = idp.CallHistory;
                return idp.RequestedUri; // The api

            case ConnectionSuccessful:
                throw new InvalidOperationException("Connection already completed");

            default:
                throw new NotImplementedException();
        }
    }

    internal static Uri GetDefaultPath(Uri uri)
    {
        if (uri.AbsolutePath == DefaultPath)
        {
            throw new E1402_NoVidiViewServerException(uri);
        }
        // Retry using our default path
        uri = new Uri(uri.Scheme + "://" + uri.Host + DefaultPath);
        return uri;
    }

    internal static Uri HandleRedirect(Uri calledUri, Uri redirectUri, List<Uri> callHistory, out PreauthenticateRequired? preauthenticateRequired)
    {
        var queryParameters = HttpUtility.ParseQueryString(redirectUri.Query);
        var type = queryParameters["idp"];
        if (!string.IsNullOrEmpty(type))
        {
            // Redirect to external Idp
            preauthenticateRequired = new PreauthenticateRequired(type, redirectUri, calledUri, callHistory);
            return redirectUri;
        }

        preauthenticateRequired = null;
        return redirectUri;
    }
}
