using VidiView.Api.Helpers;
using VidiView.Api.Exceptions;
using VidiView.Api.DataModel;
using ServiceHost = VidiView.Api.Configuration.DataModel.ServiceHost;
using ServiceHostCollection = VidiView.Api.Configuration.DataModel.ServiceHostCollection;
using System.Net.Http;

namespace VidiView.Api.Configuration;

public class ServiceHosts
{
    readonly HttpClient _http;
    readonly ApiHome _api;
    ServiceHostCollection? _cachedList;

    internal ServiceHosts(HttpClient http, ApiHome api)
    {
        _http = http;
        _api = api;
    }
    
    /// <summary>
    /// List service hosts
    /// </summary>
    /// <param name="forceReload"></param>
    /// <returns></returns>
    public async Task<ServiceHostCollection> ListAsync(bool forceReload = false)
    {
        if (!forceReload && _cachedList != null)
            return _cachedList;

        var result = await _http.GetAsync<ServiceHostCollection>(_api.Links.GetRequired(Rel.ServiceHosts));
        _cachedList = result;
        return result;
    }

    /// <summary>
    /// Reload service host
    /// </summary>
    /// <param name="serviceHostId"></param>
    /// <returns></returns>
    public Task<ServiceHost> LoadAsync(ServiceHost serviceHost)
    {
        return LoadAsync(serviceHost.Id);
    }

    /// <summary>
    /// Load a specific service host
    /// </summary>
    /// <param name="serviceHostId"></param>
    /// <returns></returns>
    public async Task<ServiceHost> LoadAsync(Guid serviceHostId)
    {
        var list = await ListAsync();
        var link = list.Links.GetRequired(Rel.Load).AsTemplatedLink();
        link.Parameters["id"].Value = serviceHostId.ToString("N");

        return await _http.GetAsync<ServiceHost>(link);
    }

    /// <summary>
    /// Save service host configuration
    /// </summary>
    /// <param name="serviceHost"></param>
    /// <returns></returns>
    /// <exception cref="E1003_AccessDeniedException"></exception>
    public async Task<ServiceHost> SaveAsync(ServiceHost serviceHost)
    {
        if (!serviceHost.Links.TryGet(Rel.Update, out var link))
        {
            // No update link, maybe it is newly created?
            if (serviceHost.Links.Exists(Rel.Self))
                throw new E1003_AccessDeniedException("Update not granted");
            link = (await ListAsync()).Links.GetRequired(Rel.Create);
        }

        var response = await _http.PutAsync(link, serviceHost);
        await response.AssertSuccessAsync();

        return response.Deserialize<ServiceHost>();
    }
}
