using VidiView.Api.Access;
using VidiView.Api.DataModel;

namespace VidiView.Api.Configuration;

public class DeviceRegistration
{
    readonly HttpClient _http;
    readonly ApiHome _api;
    
    internal DeviceRegistration(HttpClient http, ApiHome api)
    {
        _http = http;
        _api = api;
    }

    /// <summary>
    /// Set device granted state
    /// </summary>
    /// <param name="http"></param>
    /// <param name="deviceId"></param>
    /// <param name="granted"></param>
    /// <returns></returns>
    public async Task SetGrantedAsync(Guid deviceId, bool granted)
    {
        var link = _api.Links.GetRequired(Rel.GrantDevice);
        link.Parameters["deviceId"].Value = deviceId.ToString("N");
        link.Parameters["isGranted"].Value = granted.ToString();

        var response = await _http.PutAsync(link, null);
    }

    /// <summary>
    /// Delete device registration
    /// </summary>
    /// <param name="http"></param>
    /// <param name="deviceId"></param>
    /// <param name="erase">Erase the device. Used for testing purposes only</param>
    /// <returns></returns>
    public async Task DeleteAsync(Guid deviceId, bool erase)
    {
        var link = _api.Links.GetRequired(Rel.DeleteDevice);

        link.Parameters["deviceId"].Value = deviceId.ToString("N");
        link.Parameters["eraseRecord"].Value = erase.ToString();

        var response = await _http.DeleteAsync(link.ToUrl());
        await response.AssertSuccessAsync();
    }
}
