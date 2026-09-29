using VidiView.Api.Helpers;
using VidiView.Api.DataModel;
using System.Net.Http;

namespace VidiView.Api.Configuration;

/// <summary>
/// This class provides methods to manage devices registered with the VidiView system. It allows setting the 
/// granted state of a device and deleting device registrations.
/// </summary>
public class DeviceManager
{
    readonly HttpClient _http;
    readonly ApiHome _api;
    
    internal DeviceManager(HttpClient http, ApiHome api)
    {
        _http = http;
        _api = api;
    }

    /// <summary>
    /// List all registered devices
    /// </summary>
    /// <param name="onlyNewDevices">Only list new devices that are not granted access yet</param>
    /// <returns></returns>
    public async Task<IReadOnlyList<ClientDevice>> ListAsync(bool onlyNewDevices = true)
    {
        var link = _api.Links.GetRequired(Rel.ClientDevices).AsTemplatedLink();
        link.Parameters["includeGrantedDevices"].Value = (!onlyNewDevices) ? "0" : "1";
        
        var result = await _http.GetAsync<ClientDeviceCollection>(link).ConfigureAwait(false);
        return result.Items;
    }

    /// <summary>
    /// Set device granted state
    /// </summary>
    /// <param name="deviceId"></param>
    /// <param name="granted"></param>
    /// <param name="deviceName">Override the device name</param>
    /// <returns></returns>
    public async Task SetGrantedAsync(Guid deviceId, bool granted, string? deviceName = null)
    {
        var link = _api.Links.GetRequired(Rel.GrantDevice).AsTemplatedLink();
        link.Parameters["deviceId"].Value = deviceId.ToString("N");
        link.Parameters["isGranted"].Value = granted.ToString();
        link.Parameters["deviceName"].Value = deviceName;

        var response = await _http.PutAsync(link, null);
        await response.AssertSuccessAsync();
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
        var link = _api.Links.GetRequired(Rel.DeleteDevice).AsTemplatedLink();

        link.Parameters["deviceId"].Value = deviceId.ToString("N");
        link.Parameters["eraseRecord"].Value = erase.ToString();

        var response = await _http.DeleteAsync(link.ToUrl());
        await response.AssertSuccessAsync();
    }
}
