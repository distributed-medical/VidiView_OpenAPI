using System.Net.Http.Headers;
using VidiView.Api.Helpers;
using VidiView.Api.DataModel;
using VidiView.Api.Exceptions;
using System.Net.Http;
using System.Text;

namespace VidiView.Api.Authentication;
public class UsernamePasswordAuthenticator : IAuthenticator
{
    readonly HttpClient _http;

    public UsernamePasswordAuthenticator(HttpClient http)
    {
        _http = http;
    }

    public async Task<bool> IsSupportedAsync()
    {
        return (await _http.HomeAsync())
            .AssertRegistered()
            .Links.Exists(Rel.AuthenticatePassword);
    }

    public User? User { get; private set; }
    public AuthToken? Token { get; private set; }
    
    /// <summary>
    /// Authenticate with VidiView Server using username and password
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    /// <remarks>If successful, an access token is set on the HttpClient</remarks>
    public async Task AuthenticateAsync(string username, string password)
    {
        var api = await _http.HomeAsync();
        if (api.IsAuthenticated())
            throw new InvalidOperationException("Already authenticated");
        api.AssertRegistered();

        try
        {
            if (!api.Links.TryGet(Rel.AuthenticatePassword, out var link))
                throw new E1813_LogonMethodNotAllowedException("Username/password authentication is not enabled");

            _http.DefaultRequestHeaders.Authorization = CreateBasicAuthenticationHeader(username, password);

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
        User = null;
        Token = null;
        _http.DefaultRequestHeaders.Authorization = null;
    }
}
