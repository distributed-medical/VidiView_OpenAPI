using VidiView.Api.Helpers;
using VidiView.Api.DataModel;

namespace VidiView.Api.Configuration;

public class VidiViewConfigurator
{
    readonly HttpClient _http;

    public VidiViewConfigurator(HttpClient http)
    {
        _http = http;
    }

    /// <summary>
    /// Initialize the configurator
    /// </summary>
    /// <returns></returns>
    public async Task InitializeAsync()
    {
        var api = await _http.HomeAsync();
        var link = api.Links.GetRequired(Rel.Configuration);
        Home = await _http.GetAsync<ApiHome>(link);

        DeviceRegistration = new DeviceRegistration(_http, Home);
        Settings = new SettingsRepository(_http, Home);
        ServiceHosts = new ServiceHosts(_http, Home);
        Users = new UserManager(_http, Home);
    }

    public HttpClient Http => _http;

    /// <summary>
    /// Server information
    /// </summary>
    public ApiHome? Home { get; private set; }

    /// <summary>
    /// Device registration 
    /// </summary>
    public DeviceRegistration DeviceRegistration { get; private set; }

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
    public UserManager Users { get; private set; }
}
