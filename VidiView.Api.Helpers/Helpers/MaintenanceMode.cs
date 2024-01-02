using System.Net.Http;
using VidiView.Api.DataModel;
using VidiView.Api.Exceptions;

namespace VidiView.Api.Helpers;

public static class MaintenanceMode
{
    /// <summary>
    /// Check if the response failed due to maintenance mode
    /// </summary>
    /// <param name="http"></param>
    /// <param name="response"></param>
    /// <returns></returns>
    /// <exception cref="E1405_ServiceMaintenanceModeException">Thrown if the server is in maintenance mode</exception>
    public static async Task ThrowIfMaintenanceModeAsync(HttpResponseMessage response)
    {
        if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
        {
            // This indicates the computer is responding, but no service is found on the expected url
            MaintenanceInfo? maintenanceMode = null;
            try
            {
                using var http = new HttpClient();
                maintenanceMode = await GetMaintenanceModeState(http, response.RequestMessage.RequestUri);
            }
            catch
            {
                // Ignore
            }

            if (maintenanceMode?.MaintenanceMode == true)
            {
                // The service is currently in maintenance mode. Throw appropriate error
                throw new E1405_ServiceMaintenanceModeException(maintenanceMode.Message ?? "Maintenance mode", maintenanceMode.Until);
            }
        }
    }

    public static async Task<MaintenanceInfo?> GetMaintenanceModeState(HttpClient http)
    {
        var uri = http.BaseAddress;
        if (uri == null)
            throw new ArgumentException("No BaseAddress provided on the HttpClient");
        return await GetMaintenanceModeState(http, uri);
    }

    public static async Task<MaintenanceInfo?> GetMaintenanceModeState(HttpClient http, Uri uri)
    {
        var maintenanceUri = new Uri($"{uri.Scheme}://{uri.Host}{(uri.IsDefaultPort ? "" : $":{uri.Port}")}/vidiview/maintenance-mode/1/info/"); // Note the trailing slash, to get a compliant result!!

        // Default to a really short timeout
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));

        try
        {
            var result = await http.GetAsync(maintenanceUri, HttpCompletionOption.ResponseContentRead, cts.Token);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // There is a maintenance message for us
                return result.Deserialize<MaintenanceInfo>();
            }
        }
        catch
        {
        }

        return null;
    }
}
