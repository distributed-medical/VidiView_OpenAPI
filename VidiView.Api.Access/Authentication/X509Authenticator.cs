using System.Net.Http.Headers;
using VidiView.Api.DataModel;
using VidiView.Api.DataModel.Exceptions;

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
    /// Authenticate with VidiView Server using current Windows credentials
    /// </summary>
    /// <returns></returns>
    /// <remarks>If successfull, an access token is set on the HttpClient</remarks>
    public async Task AuthenticateAsync()
    {
        var api = await _http.HomeAsync();
        if (api.IsAuthenticated())
            throw new InvalidOperationException("Already authenticated");
        api.AssertRegistered();

        try
        {
            if (!api.Links.TryGet(Rel.AuthenticateWindows, out var link))
                throw new E1813_LogonMethodNotAllowedException("Windows authentication is not supported");

            User = await _http.GetAsync<User>(link);
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
    }

    /// <summary>
    /// Clear authentication
    /// </summary>
    void Clear()
    {
        User = null;
        Token = null;
    }
}
