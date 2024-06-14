using System.Net.Http.Headers;
using VidiView.Api.Helpers;
using VidiView.Api.DataModel;
using VidiView.Api.Exceptions;
using System.Net.Http;
using System.Text;

namespace VidiView.Api.Authentication;
public class PinCodeAuthenticator
{
    readonly HttpClient _http;
    Link? _authenticationLink;

    public PinCodeAuthenticator(HttpClient http)
    {
        _http = http;
        IsSupported = http.CachedHome()?.Links.Exists(Rel.AuthenticatePinEnabled) == true;
    }

    public bool IsSupported { get; private set; }
    public User? User { get; private set; }
    public AuthToken? Token { get; private set; }

    /// <summary>
    /// Returns pin code state for the user
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public async Task<LoginPin> GetStateAsync(string username)
    {
        var api = await _http.HomeAsync();
        if (api.IsAuthenticated())
            throw new InvalidOperationException("Already authenticated");

        if (!api.Links.TryGet(Rel.AuthenticatePinEnabled, out var link))
            throw new E1813_LogonMethodNotAllowedException("Pin authentication not supported");

        var tl = link.AsTemplatedLink();

        if (!tl.TrySetParameterValue("username", username))
            throw new Exception("The expected parameter could not be set");

        var result = await _http.GetAsync<LoginPin>(tl);
        result.Links.TryGet(Rel.AuthenticatePin, out _authenticationLink);

        return result;
    }

    /// <summary>
    /// Authenticate with VidiView Server using username and pin code
    /// </summary>
    /// <param name="username"></param>
    /// <param name="pin"></param>
    /// <returns></returns>
    /// <remarks>If successful, an access token is set on the HttpClient</remarks>
    public async Task AuthenticateAsync(string username, string pin)
    {
        var api = await _http.HomeAsync();
        if (api.IsAuthenticated())
            throw new InvalidOperationException("Already authenticated");
        api.AssertRegistered();

        try
        {
            if (_authenticationLink == null)
                await GetStateAsync(username);

            if (_authenticationLink == null)
                throw new E1813_LogonMethodNotAllowedException("Username/pin authentication is not enabled");

            _http.DefaultRequestHeaders.Authorization = CreateBasicAuthenticationHeader(username, pin);

            User = await _http.GetAsync<User>(_authenticationLink);
            var link = User.Links.GetRequired(Rel.IssueSamlToken) ?? throw new NotSupportedException("This server does not support issuing SAML tokens");

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

    private static AuthenticationHeaderValue CreateBasicAuthenticationHeader(string username, string password)
    {
        ArgumentNullException.ThrowIfNull(username, nameof(username));
        if (username.Contains(':'))
            throw new ArgumentException("Username may not contain the colon (:) character");

        var bytes = Encoding.UTF8.GetBytes($"{username}:{password}");
        return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));
    }

    /// <summary>
    /// Clear authentication
    /// </summary>
    void Clear()
    {
        _http.DefaultRequestHeaders.Authorization = null;
        User = null;
        Token = null;
    }
}
