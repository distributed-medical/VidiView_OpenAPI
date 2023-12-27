using IdentityModel.Client;
using System.Net.Http.Headers;
using VidiView.Api.DataModel;

namespace VidiView.Api.Helpers;

/// <summary>
/// This class provides extensions to execute common
/// http methods using a Hal-link instead of Uri.
/// </summary>
public static class HttpMethodExtensions
{
    /// <summary>
    /// Helper method to get response, verify success and then deserialize result
    /// </summary>
    /// <typeparam name="T">The type to deserialize</typeparam>
    /// <param name="http"></param>
    /// <param name="link">The link to read</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns></returns>
    public static Task<T> GetAsync<T>(this HttpClient http, TemplatedLink link, CancellationToken? cancellationToken = null)
    {
        return GetAsync<T>(http, link.ToUrl(), cancellationToken);
    }

    /// <summary>
    /// Helper method to get response, verify success and then deserialize result
    /// </summary>
    /// <typeparam name="T">The type to deserialize</typeparam>
    /// <param name="http"></param>
    /// <param name="requestUri">The url to read</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns></returns>
    public static async Task<T> GetAsync<T>(this HttpClient http, string requestUri, CancellationToken? cancellationToken = null)
    {
        var response = await http.GetAsync(requestUri, cancellationToken ?? CancellationToken.None);
        await response.AssertSuccessAsync();

        if (typeof(T) == typeof(string))
        {
            // Don't deserialize
            return (T)(object)await response.Content.ReadAsStringAsync();
        }
        else
        {
            return response.Deserialize<T>();
        }
    }

    public static Task<HttpContentStream> GetStreamAsync(this HttpClient http, TemplatedLink link, RangeHeaderValue? range = null, CancellationToken? cancellationToken = null)
    {
        return GetStreamAsync(http, link.ToUrl(), range, cancellationToken);
    }

    public static async Task<HttpContentStream> GetStreamAsync(this HttpClient http, string requestUri, RangeHeaderValue? range = null, CancellationToken? cancellationToken = null)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(requestUri));
        request.Headers.Range = range;

        try
        {
            var response = await http.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken ?? CancellationToken.None);
            await response.AssertSuccessAsync();
            return await HttpContentStream.CreateFromResponse(http, response);
        }
        catch (Exception ex)
        {
            //if (IsConnectionRefused(ex))
            //    throw new E1404_ServiceUnavailableException(uri, ex);
            throw;
        }
    }

    public static Task<HttpResponseMessage> DeleteAsync(this HttpClient http, TemplatedLink link, CancellationToken? cancellationToken = null)
    {
        return http.DeleteAsync(link.ToUrl(), cancellationToken ?? CancellationToken.None);
    }

    public static Task<HttpResponseMessage> HeadAsync(this HttpClient http, TemplatedLink link, CancellationToken? cancellationToken = null)
    {
        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Head,
            RequestUri = (Uri)link,
        };

        return http.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken ?? CancellationToken.None);
    }

    public static Task<HttpResponseMessage> PatchAsync(this HttpClient http, TemplatedLink link, object? content, CancellationToken? cancellationToken = null)
    {
        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Patch,
            RequestUri = (Uri)link,
            Content = HttpContentFactory.CreateBody(content),
        };

        return http.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken ?? CancellationToken.None);
    }

    public static Task<HttpResponseMessage> PostAsync(this HttpClient http, TemplatedLink link, object? content, CancellationToken? cancellationToken = null)
    {
        return http.PostAsync(link.ToUrl(), HttpContentFactory.CreateBody(content), cancellationToken ?? CancellationToken.None);
    }

    public static Task<HttpResponseMessage> PutAsync(this HttpClient http, TemplatedLink link, object? content, CancellationToken? cancellationToken = null)
    {
        return http.PutAsync(link.ToUrl(), HttpContentFactory.CreateBody(content), cancellationToken ?? CancellationToken.None);
    }
}
