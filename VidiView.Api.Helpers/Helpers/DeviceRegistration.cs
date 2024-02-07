using VidiView.Api.DataModel;

namespace VidiView.Api.Helpers;

/// <summary>
/// This class can be used to register a device with a VidiView Server
/// </summary>
public static class DeviceRegistration
{
    /// <summary>
    /// Register client device with server
    /// </summary>
    /// <param name="http"></param>
    /// <param name="device"></param>
    /// <returns></returns>
    public static async Task<ClientDevice> RegisterAsync(System.Net.Http.HttpClient http, ClientDevice device)
    {
        var api = await http.HomeAsync();
        return await RegisterAsync(http, api, device);
    }

    /// <summary>
    /// Register client device with server
    /// </summary>
    /// <param name="http"></param>
    /// <param name="api"></param>
    /// <param name="device"></param>
    /// <returns></returns>
    public static async Task<ClientDevice> RegisterAsync(System.Net.Http.HttpClient http, ApiHome api, ClientDevice device)
    {
        try
        {
            var link = api.Links.GetRequired(Rel.RegisterClientDevice);

            var response = await http.PutAsync(link, device).ConfigureAwait(false);
            await response.AssertSuccessAsync().ConfigureAwait(false);
            var result = await response.DeserializeAsync<ClientDevice>().ConfigureAwait(false);
            return result;
        }
        finally
        {
            http.InvalidateHome();
        }

    }

#if WINRT
    /// <summary>
    /// Register client device with server
    /// </summary>
    /// <param name="http"></param>
    /// <param name="device"></param>
    /// <returns></returns>
    public static async Task<ClientDevice> RegisterAsync(Windows.Web.Http.HttpClient http, ClientDevice device)
    {
        var api = await http.HomeAsync();
        return await RegisterAsync(http, api, device);
    }

    /// <summary>
    /// Register client device with server
    /// </summary>
    /// <param name="http"></param>
    /// <param name="api"></param>
    /// <param name="device"></param>
    /// <returns></returns>
    public static async Task<ClientDevice> RegisterAsync(Windows.Web.Http.HttpClient http, ApiHome api, ClientDevice device)
    {
        var link = api.Links.GetRequired(Rel.RegisterClientDevice);

        var response = await http.PutAsync(link, device);
        await response.AssertSuccessAsync();
        var result = response.Deserialize<ClientDevice>();
        
        return result;
    }
#endif
}
