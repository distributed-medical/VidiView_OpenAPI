using VidiView.Api.DataModel;

namespace VidiView.Api.Access;

/// <summary>
/// This class can be used to register a device with a VidiView Server
/// </summary>
public static class DeviceRegistrationHelper
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
        var result = await http.GetAsync(""); // Utilizes BaseAddress
        var home = result.AssertSuccess().Deserialize<ApiHome>();
        var link = home!.Links.GetRequired(Rel.RegisterClientDevice);

        var response = await http.PutAsync(link.ToUrl(), HttpContentFactory.CreateBody(device));
        return response.AssertSuccess().Deserialize<ClientDevice>();
    }
}
