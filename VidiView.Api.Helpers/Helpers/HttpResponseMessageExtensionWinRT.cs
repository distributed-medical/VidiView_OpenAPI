#if WINRT
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using VidiView.Api.DataModel;
using VidiView.Api.Exceptions;
using VidiView.Api.Serialization;
using Windows.Web.Http;

namespace VidiView.Api.Helpers;

public static class HttpResponseMessageExtensionWinRT
{
    public static JsonSerializerOptions Options { get; set; } = VidiViewJson.DefaultOptions;

    /// <summary>
    /// Deserialize response as specific entity type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="response"></param>
    /// <returns></returns>
    public static T Deserialize<T>(this HttpResponseMessage response)
    {
        // Deserialize as Json
        var stream = response.Content.ReadAsInputStreamAsync().GetResults(); // Note sync call!
        return JsonSerializer.Deserialize<T>(stream.AsStreamForRead(), Options)!;
    }

    /// <summary>
    /// Check if response is successful, otherwise throw appropriate exception
    /// </summary>
    /// <param name="response"></param>
    public static async Task AssertSuccessAsync(this HttpResponseMessage response)
    {
        ArgumentNullException.ThrowIfNull(response, nameof(response));

        if (response.IsSuccessStatusCode == true)
        {
            // Success. No more processing required
            return;
        }
        else
        {
            // Check if we have any additional error information
            Exception? exc = null;
            try
            {
                var error = response.Deserialize<ErrorDetails>();
                if (error != null)
                {
                    exc = VidiViewException.Factory((System.Net.HttpStatusCode)(int)response.StatusCode, error);
                }
            }
            catch
            {
                //Debug.Assert(false, "Failed to deserialize ErrorDetails");
            }

            if (exc != null)
            {
                throw exc;
            }

            await MaintenanceMode.ThrowIfMaintenanceModeAsync((System.Net.HttpStatusCode)(int)response.StatusCode, response.RequestMessage.RequestUri);

            switch (response.StatusCode)
            {
                case HttpStatusCode.RequestTimeout:
                    throw new E1004_TimeoutException("Request timeout");

                case HttpStatusCode.MethodNotAllowed:
                    throw new E1030_NotSupportedException("This method is not implemented in the server service");

                case HttpStatusCode.RequestedRangeNotSatisfiable:
                    throw new ArgumentOutOfRangeException("Position is out of range");

                case HttpStatusCode.Forbidden:
                    throw new E1003_AccessDeniedException(!string.IsNullOrEmpty(response.ReasonPhrase) ? response.ReasonPhrase : "403 Forbidden");

                default:
                    // Just throw default error
                    response.EnsureSuccessStatusCode();

                    Debug.Assert(false, "We should not get here ever..");
                    throw new Exception("E_FAIL");

            }
        }
    }
}
#endif
