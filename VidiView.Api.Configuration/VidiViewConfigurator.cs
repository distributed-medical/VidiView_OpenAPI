using VidiView.Api.Helpers;
using VidiView.Api.DataModel;

namespace VidiView.Api.Configuration;

public class VidiViewConfigurator
{
    readonly HttpClient _http;
    ApiHome? _configurationHome;

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
        _configurationHome = await _http.GetAsync<ApiHome>(link);

        DeviceRegistration = new DeviceRegistration(_http, _configurationHome);
        Settings = new SettingsRepository(_http, _configurationHome);
        ServiceHosts = new ServiceHosts(_http, _configurationHome);
    }

    /// <summary>
    /// Server information
    /// </summary>
    public ApiHome? Home => _configurationHome;

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
}
