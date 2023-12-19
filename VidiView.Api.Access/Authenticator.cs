using System.Net.Http.Headers;
using VidiView.Api.DataModel;
using VidiView.Api.DataModel.Exceptions;

namespace VidiView.Api.Access;
public class Authenticator
{
    readonly HttpClient _http;

    public Authenticator(HttpClient http)
    {
        _http = http;
    }

    /// <summary>
    /// Clear authentication
    /// </summary>
    public void Clear()
    {
        _http.DefaultRequestHeaders.Authorization = null;
        AuthenticatedUser = null;
        Token = null;
    }

    /// <summary>
    /// Authenticate with VidiView Server using username and password
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    /// <remarks>If successfull, an access token is set on the HttpClient</remarks>
    public async Task UsernamePasswordAsync(string username, string password)
    {
        Clear();

        try
        {
            var api = await _http.HomeAsync(forceReload: true);

            if (!api.Links.TryGet(Rel.AuthenticatePassword, out var link))
            {
                if (!api.Links.Exists(Rel.ClientDeviceRegistration))
                    throw new Exception("Device is not registered");

                throw new E1813_LogonMethodNotAllowedException("Username/password authentication is not enabled");
            }

            _http.DefaultRequestHeaders.Authorization =
                new BasicAuthenticationHeaderValue(username, password);

            AuthenticatedUser = await _http.GetAsync<User>(link);
            link = AuthenticatedUser.Links.GetRequired(Rel.IssueSamlToken) ?? throw new NotSupportedException("This server does not support issuing SAML tokens");

            Token = await _http.GetAsync<AuthToken>(link);
            _http.DefaultRequestHeaders.Authorization 
                = new AuthenticationHeaderValue("Bearer", Token.Token);

            api = await _http.HomeAsync(forceReload: true);
        }
        catch
        {
            _http.DefaultRequestHeaders.Authorization = null;
            throw;
        }
    }

    public User? AuthenticatedUser { get; private set; }
    public AuthToken? Token { get; private set; }
}
