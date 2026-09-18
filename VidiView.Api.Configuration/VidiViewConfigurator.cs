using VidiView.Api.Helpers;
using VidiView.Api.DataModel;
using System.Net.Http;

namespace VidiView.Api.Configuration;

/// <summary>
/// This is a helper class to manage the configuration of a VidiView server
/// </summary>
public class VidiViewConfigurator
{
    readonly HttpClient _http;

    /// <summary>
    /// Create a new instance of the configurator
    /// </summary>
    /// <param name="http">The HttpClient to use for API requests</param>
    public VidiViewConfigurator(HttpClient http)
    {
        _http = http;
    }

    /// <summary>
    /// Initialize the configurator. Ensure the HttpClient is authenticated before calling 
    /// this method. This method will retrieve the API home document and initialize the 
    /// various managers.
    /// </summary>
    /// <returns></returns>
    public async Task InitializeAsync()
    {
        var api = await _http.HomeAsync();
        var link = api.Links.GetRequired(Rel.Configuration);
        Home = await _http.GetAsync<ApiHome>(link);

        DeviceRegistration = new DeviceManager(_http, Home);
        Departments = new DepartmentManager(_http, Home);   
        Settings = new SettingsRepository(_http, Home);
        ServiceHosts = new ServiceHosts(_http, Home);
        UserAccounts = new UserAccountManager(_http, Home);
    }

    public HttpClient Http => _http;

    /// <summary>
    /// Server information
    /// </summary>
    public ApiHome? Home { get; private set; }

    /// <summary>
    /// Device registration 
    /// </summary>
    public DeviceManager DeviceRegistration { get; private set; }

    /// <summary>
    /// Department configuration
    /// </summary>
    public DepartmentManager Departments { get; private set; }

    /// <summary>
    /// Settings repository
    /// </summary>
    public SettingsRepository Settings { get; private set; }

    /// <summary>
    /// Service hosts
    /// </summary>
    public ServiceHosts ServiceHosts { get; private set; }

    /// <summary>
    /// Users
    /// </summary>
    public UserAccountManager UserAccounts { get; private set; }
}
