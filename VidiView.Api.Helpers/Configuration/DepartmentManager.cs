using System.Net.Http;
using VidiView.Api.DataModel;
using VidiView.Api.Helpers;

namespace VidiView.Api.Configuration;
public class DepartmentManager
{
    readonly HttpClient _http;
    readonly ApiHome _api;
    LinkCollection? _links;

    public DepartmentManager(HttpClient http, ApiHome api)
    {
        _http = http;
        _api = api;
    }

    /// <summary>
    /// List all users in the system
    /// </summary>
    /// <returns></returns>
    public async Task<DepartmentCollection> ListAsync()
    {
        var link = _api.Links.GetRequired(Rel.Departments);
        var result = await _http.GetAsync<DepartmentCollection>(link).ConfigureAwait(false);
        _links = result.Links;
        return result;
    }
}
