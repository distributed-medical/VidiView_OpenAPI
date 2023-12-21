using VidiView.Api.DataModel;

namespace VidiView.Api.Access;

public static class MaintenanceModeHelper
{
    public static async Task<MaintenanceInfo?> GetMaintenanceModeState(HttpClient client)
    {
        var uri = client.BaseAddress;
        if (uri == null)
            throw new ArgumentException("No BaseAddress provided on the HttpClient");

        var maintenanceUri = new Uri($"{uri.Scheme}://{uri.Host}{(uri.IsDefaultPort ? "" : $":{uri.Port}")}/vidiview/maintenance-mode/1/info/"); // Note the trailing slash, to get a compliant result!!

        // Default to a really short timeout
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));

        try
        {
            var result = await client.GetAsync(maintenanceUri, HttpCompletionOption.ResponseContentRead, cts.Token);
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
