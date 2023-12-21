using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using VidiView.Api.DataModel;

namespace VidiView.Api.Access.Authentication;
public class X509Authenticator : IAuthenticator
{
    readonly HttpClient _http;

    public X509Authenticator(HttpClient http)
    {
        _http = http;
        IsSupported = http.CachedHome()?.Links.Exists(Rel.TrustedIssuers) == true;
    }

    public bool IsSupported { get; private set; }
    public User? User { get; private set; }
    public AuthToken? Token { get; private set; }

    /// <summary>
    /// Return a collection of trusted X509 issuers. Null if X509 authentication isn't supported
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TrustedIssuerCollection?> TrustedIssuersAsync()
    {
        var api = await _http.HomeAsync();
        if (api.IsAuthenticated())
            throw new InvalidOperationException("Already authenticated");
        api.AssertRegistered();

        if (api.Links.TryGet(Rel.TrustedIssuers, out var link))
        {
            return await _http.GetAsync<TrustedIssuerCollection>(link);
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Download trusted certificate in PEM format
    /// </summary>
    /// <param name="trustedIssuer"></param>
    /// <returns></returns>
    public async Task<string> DownloadAsync(TrustedIssuer trustedIssuer)
    {
        if (trustedIssuer.Links.TryGet(Rel.Enclosure, out var link))
            return await _http.GetAsync<string>(link);
        else
            throw new NotSupportedException("The server does not support downloading of this certificate");
    }

    /// <summary>
    /// Authenticate with VidiView Server using a specific client certificate
    /// </summary>
    /// <returns></returns>
    /// <remarks>If successfull, an access token is set on the HttpClient</remarks>
    public async Task AuthenticateAsync(X509Certificate certificate)
    {
        var api = await _http.HomeAsync();
        if (api.IsAuthenticated())
            throw new InvalidOperationException("Already authenticated");
        api.AssertRegistered();

        // HACK!
        // To set the client certificate, we need to access the message handler
        var messageHandler = GetPrivateMessageHandler(_http);
        var handler = GetHttpClientHandler(messageHandler);

        // Add this client certificate
        handler.ClientCertificates.Clear();
        handler.ClientCertificates.Add(certificate);

        try
        {
            Exception? authenticationException = null;

            // If the server supports Windows AD X509 authentication, try this method first 
            if (api.Links.TryGet(Rel.AuthenticateX509Windows, out var link))
            {
                try
                {
                    User = await _http.GetAsync<User>(link);
                    authenticationException = null;
                }
                catch (Exception exc)
                {
                    authenticationException = exc;
                }
            }

            if (User == null && api.Links.TryGet(Rel.AuthenticateX509, out link))
            {
                try
                {
                    User = await _http.GetAsync<User>(link);
                    authenticationException = null;
                }
                //catch (E1816_CertificateNotMappedToAnyUserException exc)
                //{
                //    authenticationException = exc;
                //}
                catch (Exception exc)
                {
                    authenticationException ??= exc;
                }
            }

            if (authenticationException != null)
                throw authenticationException;

            link = User.Links.GetRequired(Rel.IssueSamlToken) ?? throw new NotSupportedException("This server does not support issuing SAML tokens");

            Token = await _http.GetAsync<AuthToken>(link);
            _http.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", Token.Token);

            _http.InvalidateHome();
        }
        catch
        {
            Clear();
            throw;
        }
        finally
        {
            handler.ClientCertificates.Clear();
        }
    }

    /// <summary>
    /// Clear authentication
    /// </summary>
    void Clear()
    {
        User = null;
        Token = null;
    }

    static HttpClientHandler GetHttpClientHandler(HttpMessageHandler messageHandler)
    {
        while (messageHandler is DelegatingHandler dh)
            messageHandler = dh.InnerHandler;

        return messageHandler as HttpClientHandler
            ?? throw new NotSupportedException("The HttpClient is expected to have an HttpClientHandler");
    }

    static HttpMessageHandler GetPrivateMessageHandler(HttpClient http)
    {
        var fi = typeof(HttpMessageInvoker).GetField("_handler", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?? throw new Exception("The private field _handler is expected to exist in HttpMessageInvoker when running .NET7");

        var handler = fi.GetValue(http)
            ?? throw new Exception("The private field _handler does not contain any message handler");

        return handler as HttpMessageHandler
            ?? throw new Exception("The private field _handler does not contain any message handler of the expected type");
    }
}
