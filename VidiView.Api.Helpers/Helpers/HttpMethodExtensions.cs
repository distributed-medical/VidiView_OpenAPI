using System.Net.Http;
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
    /// <param name="link">The url to read</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns></returns>
    public static async Task<T> GetAsync<T>(this HttpClient http, TemplatedLink link, CancellationToken? cancellationToken = null)
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

    /// <summary>
    /// Get a stream from the server. This may support seeking
    /// </summary>
    /// <param name="http"></param>
    /// <param name="link"></param>
    /// <param name="range"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<HttpContentStream> GetStreamAsync(this HttpClient http, TemplatedLink link, RangeHeaderValue? range = null, CancellationToken? cancellationToken = null)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get,  (Uri)link);
        request.Headers.Range = range;

        try
        {
            var response = await http.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken ?? CancellationToken.None);
            await response.AssertSuccessAsync();
            return await HttpContentStream.CreateFromResponse(http, response);
        }
        catch
        {
            //if (IsConnectionRefused(ex))
            //    throw new E1404_ServiceUnavailableException(uri, ex);
            throw;
        }
    }

    /// <summary>
    /// Helper method to delete a resource indicated by a link
    /// </summary>
    /// <param name="http"></param>
    /// <param name="link"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The raw response, which should be checked for success</returns>
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

    /// <summary>
    /// Helper method to patch a resource indicated by a link with a specific content
    /// </summary>
    /// <param name="http"></param>
    /// <param name="link"></param>
    /// <param name="content"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The raw response, which should be checked for success</returns>
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

    /// <summary>
    /// Helper method to create a resource indicated by a link with a specific content
    /// </summary>
    /// <param name="http"></param>
    /// <param name="link"></param>
    /// <param name="content"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The raw response, which should be checked for success</returns>
    public static Task<HttpResponseMessage> PostAsync(this HttpClient http, TemplatedLink link, object? content, CancellationToken? cancellationToken = null)
    {
        return http.PostAsync(link.ToUrl(), HttpContentFactory.CreateBody(content), cancellationToken ?? CancellationToken.None);
    }

    /// <summary>
    /// Helper method to create/update a resource indicated by a link with a specific content
    /// </summary>
    /// <param name="http"></param>
    /// <param name="link"></param>
    /// <param name="content"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The raw response, which should be checked for success</returns>
    public static Task<HttpResponseMessage> PutAsync(this HttpClient http, TemplatedLink link, object? content, CancellationToken? cancellationToken = null)
    {
        return http.PutAsync(link.ToUrl(), HttpContentFactory.CreateBody(content), cancellationToken ?? CancellationToken.None);
    }
}
