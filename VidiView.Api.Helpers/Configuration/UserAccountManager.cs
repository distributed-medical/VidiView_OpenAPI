using System.Net.Http;
using VidiView.Api.DataModel;
using VidiView.Api.Helpers;

namespace VidiView.Api.Configuration;

public class UserAccountManager
{
    readonly HttpClient _http;
    readonly ApiHome _api;
    LinkCollection? _links;

    public UserAccountManager(HttpClient http, ApiHome api)
    {
        _http = http;
        _api = api;
    }

    /// <summary>
    /// Light-weight search for users in the system. This is intended to be used for auto-complete or similar functionality.
    /// </summary>
    /// <param name="query">The query to search for. It will automatically be appended with wildcard characters if not already present.</param>
    /// <param name="onlyUsers">Set to true to only include users accounts and exclude group accounts</param>
    /// <param name="maximumHits">The maximum hits that can be returned.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>An array of user entities, with a limited set of information. 
    /// If more users than specified in <see cref="maximumHits"/> exists, null will be returned.</returns>
    /// <remarks>The returned User object may contain null for non-nullable properties.</remarks>
    public async Task<IReadOnlyList<User>> FindCandidatesAsync(string query, bool onlyUsers = false, int maximumHits = 50, CancellationToken cancellationToken = default)
    {
        var link = _api.Links.GetRequired(Rel.FindUserCandidates).AsTemplatedLink();
        link.TrySetParameterValue("query", query);
        link.TrySetParameterValue("isUser", onlyUsers ? "1" : "0");
        link.TrySetParameterValue("maxHits", maximumHits.ToString());

        var result = await _http.GetAsync<User[]>(link, cancellationToken).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// List users in the system
    /// </summary>
    /// <param name="filter">Optional filter to apply to the user list</param>
    /// <returns></returns>
    public async Task<UserCollection> ListAsync(UserAccountFilter? filter)
    {
        var link = _api.Links.GetRequired(Rel.Users).AsTemplatedLink();

        if (filter != null)
        {
            link.TrySetParameterValue("isUser", filter.OnlyUserAccounts ? "1" : "0");
            link.TrySetParameterValue("includeDeleted", filter.IncludeDeletedAccounts ? "1" : "0");
            link.TrySetParameterValue("includeImplicit", filter.IncludeImplicitAccounts ? "1" : "0");
            link.TrySetParameterValue("name", filter.Name);
            link.TrySetParameterValue("maxHits", filter.MaximumHits.ToString());
        }

        var result = await _http.GetAsync<UserCollection>(link).ConfigureAwait(false);
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
            await ListAsync(null);
        }

        var createLink = _links.GetRequired(Rel.Create);

        var response = await _http.PostAsync(createLink, user).ConfigureAwait(false);
        return await response.DeserializeAsync<User>().ConfigureAwait(false);
    }

    /// <summary>
    /// Create a new User in the VidiView system based on an Active Directory account
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<User> CreateAsync(ADAccountObject user)
    {
        if (_links == null)
        {
            // This will cache the links
            await ListAsync(null);
        }

        var createLink = _links.GetRequired(Rel.Create);

        var response = await _http.PostAsync(createLink, user).ConfigureAwait(false);
        return await response.DeserializeAsync<User>().ConfigureAwait(false);
    }

    /// <summary>
    /// Save user object and return updated instance
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<User> UpdateAsync(User user)
    {
        var link = user.Links.GetRequired(Rel.Update);

        var response = await _http.PutAsync(link, user).ConfigureAwait(false);
        await response.AssertSuccessAsync().ConfigureAwait(false);
        return await response.DeserializeAsync<User>().ConfigureAwait(false);
    }

    /// <summary>
    /// Delete a user account
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task DeleteAsync(User user)
    {
        var link = user.Links.GetRequired(Rel.Delete);

        var response = await _http.DeleteAsync(link).ConfigureAwait(false);
        await response.AssertSuccessAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Returns the domain tree including trusted domains
    /// </summary>
    /// <returns></returns>
    public async Task<IReadOnlyList<ADDomainObject>> GetDomainTreeAsync(CancellationToken cancellationToken = default)
    {
        var link = _api.Links.GetRequired(Rel.ADDomain).AsTemplatedLink();
        var result = await _http.GetAsync<ADDomainObject[]>(link, cancellationToken).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Search the Active Directory domain for user and/or group accounts
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<ADAccountObject>> GetDomainAccountsAsync(ADAccountFilter filter, CancellationToken cancellationToken = default)
    {
        var link = _api.Links.GetRequired(Rel.ADAccount).AsTemplatedLink();
        link.TrySetParameterValue("domain", filter.DomainName);
        link.TrySetParameterValue("accountName", filter.AccountName);
        link.TrySetParameterValue("type", filter.Type == (ADAccountType.User | ADAccountType.Group) ? "any" : filter.Type.ToString());
        link.TrySetParameterValue("maxHits", filter.MaximumHits.ToString());

        var result = await _http.GetAsync<ADAccountObject[]>(link, cancellationToken).ConfigureAwait(false);
        return result;
    }

}
