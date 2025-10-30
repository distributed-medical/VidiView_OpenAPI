using System.Net.Http.Headers;
using VidiView.Api.Helpers;
using VidiView.Api.DataModel;
using VidiView.Api.Exceptions;
using System.Net.Http;

namespace VidiView.Api.Authentication;
public class WindowsAuthenticator : IAuthenticator
{
    readonly HttpClient _http;

    public WindowsAuthenticator(HttpClient http)
    {
        _http = http;
    }

    public async Task<bool> IsSupportedAsync()
    {
        var start = (await _http.HomeAsync())
            .AssertRegistered();
        return start.Links.Exists(Rel.AuthenticateWindows);
    }

    public User? User { get; private set; }
    public AuthToken? Token { get; private set; }

    /// <summary>
    /// Optional token request options
    /// </summary>
    public TokenRequest? Options { get; set; }

    /// <summary>
    /// Authenticate with VidiView Server using current Windows credentials
    /// </summary>
    /// <returns></returns>
    /// <remarks>If successful, an access token is set on the HttpClient</remarks>
    public async Task AuthenticateAsync()
    {
        var api = await _http.HomeAsync().ConfigureAwait(false); 
        if (api.IsAuthenticated())
            throw new InvalidOperationException("Already authenticated");
        api.AssertRegistered();

        try
        {
            if (!api.Links.TryGet(Rel.AuthenticateWindows, out var link))
                throw new E1813_LogonMethodNotAllowedException("Windows authentication is not supported");

            User = await _http.GetAsync<User>(link).ConfigureAwait(false);
            link = User.Links.GetRequired(Rel.RequestToken);

            var response = await _http.PostAsync(link, Options).ConfigureAwait(false);
            await response.AssertNotMaintenanceModeAsync(_http).ConfigureAwait(false);
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
