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

    public static Task<HttpResponseMessage> PutAsync(this HttpClient http, TemplatedLink link, object? content, CancellationToken? cancellationToken = null)
    {
        return PutAsync(http, link.ToUrl(), content, cancellationToken);
    }

    /// <summary>
    /// Helper method to put content using a link
    /// </summary>
    /// <param name="http"></param>
    /// <param name="link"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static async Task<HttpResponseMessage> PutAsync(this HttpClient http, string requestUri, object? content, CancellationToken? cancellationToken = null)
    {
        var response = await http.PutAsync(requestUri, HttpContentFactory.CreateBody(content), cancellationToken ?? CancellationToken.None);
        await response.AssertSuccessAsync();
        return response;
    }

    public static Task<HttpResponseMessage> PostAsync(this HttpClient http, TemplatedLink link, object? content, CancellationToken? cancellationToken = null)
    {
        return PostAsync(http, link.ToUrl(), content, cancellationToken);
    }

    public static async Task<HttpResponseMessage> PostAsync(this HttpClient http, string requestUri, object? content, CancellationToken? cancellationToken = null)
    {
        var response = await http.PostAsync(requestUri, HttpContentFactory.CreateBody(content), cancellationToken ?? CancellationToken.None);
        await response.AssertSuccessAsync();
        return response;
    }

    public static Task<HttpResponseMessage> DeleteAsync(this HttpClient http, TemplatedLink link, CancellationToken? cancellationToken = null)
    {
        return DeleteAsync(http, link.ToUrl(), cancellationToken);
    }

    public static async Task<HttpResponseMessage> DeleteAsync(this HttpClient http, string requestUri, CancellationToken? cancellationToken = null)
    {
        var response = await http.DeleteAsync(requestUri, cancellationToken ?? CancellationToken.None);
        await response.AssertSuccessAsync();
        return response;
    }

    public static Task<HttpResponseMessage> HeadAsync(this HttpClient http, TemplatedLink link, CancellationToken? cancellationToken = null)
    {
        return HeadAsync(http, link.ToUrl(), cancellationToken);
    }

    public static async Task<HttpResponseMessage> HeadAsync(this HttpClient http, string requestUri, CancellationToken? cancellationToken = null)
    {
        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Head,
            RequestUri = new Uri(requestUri)
        };
        var response = await http.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken ?? CancellationToken.None);
        await response.AssertSuccessAsync();
        return response;
    }

    public static Task<HttpResponseMessage> PatchAsync(this HttpClient http, TemplatedLink link, object? content, CancellationToken? cancellationToken = null)
    {
        return PatchAsync(http, link.ToUrl(), content, cancellationToken);
    }


    public static async Task<HttpResponseMessage> PatchAsync(this HttpClient http, string requestUri, object? content, CancellationToken? cancellationToken = null)
    {
        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Patch,
            RequestUri = new Uri(requestUri)
        };
        var response = await http.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken ?? CancellationToken.None);
        await response.AssertSuccessAsync();
        return response;
    }


    static bool IsConnectionRefused(Exception ex)
    {
        return (ex.InnerException is System.Net.Sockets.SocketException sexc
            && sexc.SocketErrorCode == System.Net.Sockets.SocketError.ConnectionRefused);
    }

}
