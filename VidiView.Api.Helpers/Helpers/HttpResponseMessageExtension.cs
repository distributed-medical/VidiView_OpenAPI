using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using VidiView.Api.DataModel;
using VidiView.Api.Exceptions;
using VidiView.Api.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VidiView.Api.Helpers;

public static class HttpResponseMessageExtension
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
        var stream = response.Content.ReadAsStream();
        return JsonSerializer.Deserialize<T>(stream, Options)!;
    }

    /// <summary>
    /// Deserialize response as specific entity type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="response"></param>
    /// <returns></returns>
    public static async Task<T> DeserializeAsync<T>(this HttpResponseMessage response)
    {
        // Deserialize as Json
        var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        return (await JsonSerializer.DeserializeAsync<T>(stream, Options))!;
    }

    /// <summary>
    /// Deserializes any problem indicated by the server and rethrows as exception
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    public static async Task AssertNotProblem(this HttpResponseMessage response)
    {
        if (response.Content.Headers.ContentType?.MediaType?.Equals(ProblemDetails.ContentType, StringComparison.OrdinalIgnoreCase) == true)
        {
            ProblemDetails problem;
            try
            {
                // This is an error that can be deserialized into an exception
                string bufferedResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                problem = JsonSerializer.Deserialize<ProblemDetails>(bufferedResponse, Options)
                    ?? throw new Exception("Failed to deserialize response as ProblemDetails");
                problem.RawResponse = bufferedResponse;
            }
            catch
            {
                // Failed to deserialize as problem. This is really a problem
                throw new Exception("The server responded with an indication of a problem, but the application was unable to deserialize the details");
            }

            if (problem.Type.StartsWith(ProblemDetails.VidiViewExceptionUri))
            {
                throw VidiViewException.Factory((System.Net.HttpStatusCode)(int)response.StatusCode, problem, response.RequestMessage?.RequestUri);
            }

            throw new Exception(problem.Detail);
        }
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
            await AssertNotProblem(response);

            // Check if server is in maintenance mode
            await MaintenanceMode.ThrowIfMaintenanceModeAsync(response.StatusCode, response.RequestMessage?.RequestUri);

            // Check if this a generic error
            switch (response.StatusCode)
            {
                case HttpStatusCode.RequestTimeout:
                    throw new E1004_TimeoutException("Request timeout");

                case HttpStatusCode.MethodNotAllowed:
                    throw new E1030_NotSupportedException("This method is not implemented in the server service");

                case HttpStatusCode.RequestedRangeNotSatisfiable:
                    throw new ArgumentOutOfRangeException(null, "Position is out of range");

                case HttpStatusCode.Forbidden:
                    throw new E1003_AccessDeniedException(!string.IsNullOrEmpty(response.ReasonPhrase) ? response.ReasonPhrase : "403 Forbidden");

                case HttpStatusCode.ServiceUnavailable:
                    // No maintenance mode message presented
                    throw new E1401_NoResponseFromServerException("Service unavailable");

                default:
                    // Just throw default error
                    response.EnsureSuccessStatusCode();

                    Debug.Assert(false, "We should not get here ever..");
                    throw new Exception("E_FAIL");
            }
        }
    }
}