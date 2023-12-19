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
    /// Helper method to get and response, verify success and then deserialize result
    /// </summary>
    /// <typeparam name="T">The type to deserialize</typeparam>
    /// <param name="http"></param>
    /// <param name="link">The link to read</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns></returns>
    public static async Task<T> GetAsync<T>(this HttpClient http, Link link, CancellationToken? cancellationToken = null)
    {
        var response = await http.GetAsync(link.ToUrl(), cancellationToken ?? CancellationToken.None);
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

    public static async Task<HttpContentStream> GetStreamAsync(this HttpClient http, Link link, RangeHeaderValue? range = null, CancellationToken? cancellationToken = null)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, link.ToUri());
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

    /// <summary>
    /// Helper method to put content using a link
    /// </summary>
    /// <param name="http"></param>
    /// <param name="link"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static async Task<HttpResponseMessage> PutAsync(this HttpClient http, Link link, object? content, CancellationToken? cancellationToken = null)
    {
        var response = await http.PutAsync(link.ToUrl(), HttpContentFactory.CreateBody(content), cancellationToken ?? CancellationToken.None);
        await response.AssertSuccessAsync();
        return response;
    }

    public static async Task<HttpResponseMessage> PostAsync(this HttpClient http, Link link, object? content, CancellationToken? cancellationToken = null)
    {
        var response = await http.PostAsync(link.ToUrl(), HttpContentFactory.CreateBody(content), cancellationToken ?? CancellationToken.None);
        await response.AssertSuccessAsync();
        return response;
    }

    public static async Task<HttpResponseMessage> DeleteAsync(this HttpClient http, Link link, CancellationToken? cancellationToken = null)
    {
        var response = await http.DeleteAsync(link.ToUrl(), cancellationToken ?? CancellationToken.None);
        await response.AssertSuccessAsync();
        return response;
    }

    public static async Task<HttpResponseMessage> HeadAsync(this HttpClient http, Link link, CancellationToken? cancellationToken = null)
    {
        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Head,
            RequestUri = link.ToUri()
        };
        var response = await http.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken ?? CancellationToken.None);
        await response.AssertSuccessAsync();
        return response;
    }

    public static async Task<HttpResponseMessage> PatchAsync(this HttpClient http, Link link, object? content, CancellationToken? cancellationToken = null)
    {
        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Patch,
            RequestUri = link.ToUri()
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
