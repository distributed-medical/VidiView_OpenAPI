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
    /// <param name="baseUrl"></param>
    /// <param name="device"></param>
    /// <returns></returns>
    public static async Task<ClientDevice> RegisterAsync(HttpClient http, ClientDevice device)
    {
        var api = await http.HomeAsync();
        var link = api.Links.GetRequired(Rel.RegisterClientDevice);

        var response = await http.PutAsync(link, device);
        var result = response.Deserialize<ClientDevice>();
        http.InvalidateHome();

        return result;
    }
}
