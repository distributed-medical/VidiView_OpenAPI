using System.Net.Http;
using VidiView.Api.DataModel;
using VidiView.Api.Helpers;

namespace VidiView.Api.Configuration;
public class UserManager
{
    readonly HttpClient _http;
    readonly ApiHome _api;
    LinkCollection? _links;

    public UserManager(HttpClient http, ApiHome api)
    {
        _http = http;
        _api = api;
    }

    /// <summary>
    /// List all users in the system
    /// </summary>
    /// <returns></returns>
    public async Task<UserCollection> ListAsync()
    {
        var link = _api.Links.GetRequired(Rel.Users);
        var result = await _http.GetAsync<UserCollection>(link);
        _links = result.Links;
        return result;
    }

    /// <summary>
    /// Create a new User in the VidiView system
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<User> CreateAsync(User user)
    {
        if (_links == null)
        {
            // This will cache the links
            await ListAsync();
        }

        var createLink = _links.GetRequired(Rel.Create);

        var response = await _http.PostAsync(createLink, user);
        await response.AssertSuccessAsync();
        return response.Deserialize<User>();
    }

    /// <summary>
    /// Save user object and return updated instance
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<User> UpdateAsync(User user)
    {
        var link = user.Links.GetRequired(Rel.Update);

        var response = await _http.PutAsync(link, user);
        await response.AssertSuccessAsync();
        return response.Deserialize<User>();
    }

    /// <summary>
    /// Delete a user account
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task DeleteAsync(User user)
    {
        var link = user.Links.GetRequired(Rel.Delete);

        var response = await _http.DeleteAsync(link);
        await response.AssertSuccessAsync();
    }
}
