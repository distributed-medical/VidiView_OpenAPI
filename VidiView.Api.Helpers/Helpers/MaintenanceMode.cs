using System.Net;
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
//    [Obsolete("Use AssertNotMaintenanceModeAsync extension method on HttpResponseMessage instead")]
    public static async Task ThrowIfMaintenanceModeAsync(HttpStatusCode statusCode, Uri? requestedUri)
    {
        if (statusCode == HttpStatusCode.ServiceUnavailable && requestedUri != null)
        {
            // This indicates the computer is responding, but no service is found on the expected url
            MaintenanceInfo? maintenanceMode = null;
            try
            {
                using var http = new HttpClient();
                maintenanceMode = await GetMaintenanceInfoAsync(http, requestedUri);
            }
            catch
            {
                // Ignore
            }

            if (maintenanceMode?.MaintenanceMode == true)
            {
                // The service is currently in maintenance mode. Throw appropriate error
                throw new E1405_ServiceMaintenanceModeException(requestedUri, maintenanceMode.Message ?? "Maintenance mode", maintenanceMode.Until);
            }
        }
    }

    /// <summary>
    /// Check if the response failed due to maintenance mode
    /// </summary>
    /// <param name="response"></param>
    /// <param name="httpClient"></param>
    /// <returns></returns>
    /// <exception cref="E1405_ServiceMaintenanceModeException">Thrown if the server is in maintenance mode</exception>
    public static async Task AssertNotMaintenanceModeAsync(this HttpResponseMessage response, HttpClient httpClient)
    {
        var uri = response.RequestMessage?.RequestUri;
        if (response?.StatusCode == HttpStatusCode.ServiceUnavailable && uri != null)
        {
            // This indicates the computer is responding, but no service is found on the expected url
            MaintenanceInfo? maintenanceMode = null;
            try
            {
                maintenanceMode = await GetMaintenanceInfoAsync(httpClient, uri);
            }
            catch
            {
                // Ignore
            }

            if (maintenanceMode?.MaintenanceMode == true)
            {
                // The service is currently in maintenance mode. Throw appropriate error
                throw new E1405_ServiceMaintenanceModeException(uri, maintenanceMode.Message ?? "Maintenance mode", maintenanceMode.Until);
            }
        }
    }

    /// <summary>
    /// Query the maintenance mode endpoint, derived from the supplied uri
    /// to check if the server is currently in maintenance mode
    /// </summary>
    /// <param name="http"></param>
    /// <param name="uri"></param>
    /// <returns></returns>
    /// <remarks>The supplied uri is used to get host and port information. It is not the
    /// uri to the actual maintenance mode endpoint</remarks>
    public static async Task<MaintenanceInfo?> GetMaintenanceInfoAsync(HttpClient http, Uri uri)
    {
        // Default to a really short timeout
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));

        var maintenanceUri = GetMaintenanceModeUri(uri);
        var result = await http.GetAsync(maintenanceUri, HttpCompletionOption.ResponseContentRead, cts.Token);
        if (result.StatusCode == HttpStatusCode.OK)
        {
            // There is a maintenance message for us
            return await result.DeserializeAsync<MaintenanceInfo>();
        }

        return null;
    }

#if WINRT
    /// <summary>
    /// Query the maintenance mode endpoint, derived from the supplied uri
    /// to check if the server is currently in maintenance mode
    /// </summary>
    /// <param name="http"></param>
    /// <param name="uri"></param>
    /// <returns></returns>
    /// <remarks>The supplied uri is used to get host and port information. It is not the
    /// uri to the actual maintenance mode endpoint</remarks>
    public static async Task<MaintenanceInfo?> GetMaintenanceInfoAsync(Windows.Web.Http.HttpClient http, Uri uri)
    {
        // Default to a really short timeout
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));

        var request = new Windows.Web.Http.HttpRequestMessage()
        {
            Method = Windows.Web.Http.HttpMethod.Get,
            RequestUri = GetMaintenanceModeUri(uri),
        };

        var response = await http.SendRequestAsync(request, Windows.Web.Http.HttpCompletionOption.ResponseContentRead).AsTask(cts.Token);
        if (response.StatusCode == Windows.Web.Http.HttpStatusCode.Ok)
        {
            // There is a maintenance message for us
            return response.Deserialize<MaintenanceInfo>();
        }

        return null;
    }
#endif

    /// <summary>
    /// Returns a Uri to the maintenance mode endpoint, utilizing host and port information 
    /// from the supplied <see cref="uri"/>
    /// </summary>
    /// <param name="uri">A service uri to utilize as base for the maintenance mode endpoint</param>
    /// <returns></returns>
    public static Uri GetMaintenanceModeUri(Uri uri)
    {
        return new Uri($"{uri.Scheme}://{uri.Host}{(uri.IsDefaultPort ? "" : $":{uri.Port}")}/vidiview/maintenance-mode/1/info/"); // Note the trailing slash, to get a compliant result!!
    }

    /// <summary>
    /// Returns true if the supplied Uri points to the maintenance mode endpoint
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    public static bool IsMaintenanceModeEndPoint(Uri uri)
    {
        return uri.AbsolutePath.Contains("/vidiview/maintenance-mode/1/info/", StringComparison.CurrentCulture);
    }
}
