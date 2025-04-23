using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using VidiView.Api.Helpers;
using VidiView.Api.DataModel;
using System.Net.Http;
using VidiView.Api.Exceptions;

namespace VidiView.Api.Authentication;


/// <summary>
/// This is used to authenticate user using an X509 certificate (i.e. smart card)
/// </summary>
/// <remarks>
/// NOTICE!
/// 
/// This does not appear to be working when targeting .NET 8, but it
/// works perfectly when targeting .NET 7. This may have to be further
/// investigated!!
/// 
/// 
/// 
/// 
/// </remarks>
public class X509Authenticator : IAuthenticator
{
    readonly HttpClient _http;

    public X509Authenticator(HttpClient http)
    {
        _http = http;
    }

    public async Task<bool> IsSupportedAsync()
    {
        return (await _http.HomeAsync())
            .AssertRegistered()
            .Links.Exists(Rel.TrustedIssuers);
    }

    public bool IsSupported { get; private set; }
    public User? User { get; private set; }
    public AuthToken? Token { get; private set; }

    /// <summary>
    /// Optional token request options
    /// </summary>
    public TokenRequest? Options { get; set; }

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
    /// <remarks>If successful, an access token is set on the HttpClient</remarks>
    public async Task AuthenticateAsync(X509Certificate certificate)
    {
        var api = await _http.HomeAsync().ConfigureAwait(false);
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
                    User = await _http.GetAsync<User>(link).ConfigureAwait(false);
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
                    User = await _http.GetAsync<User>(link).ConfigureAwait(false);
                    authenticationException = null;
                }
                //catch (E1816_CertificateNotMappedToAnyUserException exc)
                //{
                //    authenticationException = exc;
                //}
                catch (Exception exc)
                {
                    if (authenticationException is not VidiViewException)
                    {
                        authenticationException = exc;
                    }
                }
            }

            if (authenticationException != null)
                throw authenticationException;

            if (User == null)
                throw new Exception("User object not retrievable");

            link = User.Links.GetRequired(Rel.RequestToken);

            var response = await _http.PostAsync(link, Options).ConfigureAwait(false);
            await response.AssertSuccessAsync().ConfigureAwait(false);
            Token = response.Deserialize<AuthToken>();

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
    /// Return certificates eligible for client authentication.
    /// This will look in the current user's personal certificate store
    /// </summary>
    public async Task<X509Certificate2Collection> EligibleClientCertificatesAsync()
    {
        using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        store.Open(OpenFlags.ReadOnly);

        return await EligibleClientCertificatesAsync(store);
    }

    /// <summary>
    /// Return certificates eligible for client authentication
    /// </summary>
    /// <param name="store">The certificate store to look in</param>
    public async Task<X509Certificate2Collection> EligibleClientCertificatesAsync(X509Store store)
    {
        var issuers = await TrustedIssuersAsync();

        // Find eligible certificates
        var result = new X509Certificate2Collection();
        if (issuers != null)
        {
            foreach (var trusted in issuers.Items)
            {
                var certs = store.Certificates
                    .Find(X509FindType.FindByIssuerDistinguishedName, trusted.DistinguishedName, true)
                    .Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, true);

                result.AddRange(certs);
            }
        }

        return result;
    }

    /// <summary>
    /// Clear authentication
    /// </summary>
    void Clear()
    {
        User = null;
        Token = null;
    }

    static HttpClientHandler GetHttpClientHandler(HttpMessageHandler? messageHandler)
    {
        while (true)
        {
            if (messageHandler is HttpClientHandler hch)
                return hch;

            if (messageHandler is DelegatingHandler dh)
            {
                messageHandler = dh.InnerHandler;
                continue;
            }

            break;
        }

        throw new NotSupportedException("The HttpClient is expected to have an HttpClientHandler");
    }

    static HttpMessageHandler GetPrivateMessageHandler(HttpClient http)
    {
        var fi = typeof(HttpMessageInvoker).GetField("_handler", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?? throw new Exception("The private field _handler is expected to exist in HttpMessageInvoker when running .NET8");

        var handler = fi.GetValue(http)
            ?? throw new Exception("The private field _handler does not contain any message handler");

        return handler as HttpMessageHandler
            ?? throw new Exception("The private field _handler does not contain any message handler of the expected type");
    }
}
