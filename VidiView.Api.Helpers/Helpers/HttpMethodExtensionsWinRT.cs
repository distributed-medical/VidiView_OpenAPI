#if WINRT
using System.Runtime.Versioning;
using VidiView.Api.DataModel;
using VidiView.Api.Exceptions;
using Windows.Web.Http;

namespace VidiView.Api.Helpers;

/// <summary>
/// This class provides extensions to execute common
/// http methods using a Hal-link instead of Uri.
/// </summary>
[SupportedOSPlatform("windows10.0.17763.0")]
public static class HttpMethodExtensionsWinRT
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
        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = (Uri)link,
        };
        
        var response = await SendRequestInternal(http, request, cancellationToken);
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
    public static async Task<HttpContentStreamWinRT> GetStreamAsync(this HttpClient http, TemplatedLink link, CancellationToken? cancellationToken = null)
    {
        return await HttpContentStreamWinRT.CreateFromUriAsync(http, (Uri)link).AsTask(cancellationToken ?? CancellationToken.None);
    }

    /// <summary>
    /// Get a stream from the server. This may support seeking
    /// </summary>
    /// <param name="http"></param>
    /// <param name="link"></param>
    /// <param name="range"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<HttpContentStreamWinRT> GetStreamAsync(this HttpClient http, TemplatedLink link, CancellationToken cancellationToken, IProgress<ulong>? progress = null)
    {
        return await HttpContentStreamWinRT.CreateFromUriAsync(http, (Uri)link).AsTask(cancellationToken, progress);
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
        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Delete,
            RequestUri = (Uri)link,
        };

        return SendRequestInternal(http, request, cancellationToken);
    }

    public static Task<HttpResponseMessage> HeadAsync(this HttpClient http, TemplatedLink link, CancellationToken? cancellationToken = null)
    {
        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Head,
            RequestUri = (Uri)link,
        };

        return SendRequestInternal(http, request, cancellationToken);
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
            Content = HttpContentFactoryWinRT.CreateBody(content),
        };

        return SendRequestInternal(http, request, cancellationToken);
    }

    /// <summary>
    /// Helper method to create a resource indicated by a link with a specific content
    /// </summary>
    /// <param name="http"></param>
    /// <param name="link"></param>
    /// <param name="content"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The raw response, which should be checked for success</returns>
    public static Task<HttpResponseMessage> PostAsync(this HttpClient http, TemplatedLink link, object? content, CancellationToken? cancellationToken = null, IProgress<HttpProgress>? progress = null)
    {
        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            RequestUri = (Uri)link,
            Content = HttpContentFactoryWinRT.CreateBody(content),
        };

        return SendRequestInternal(http, request, cancellationToken, progress);
    }

    /// <summary>
    /// Helper method to create/update a resource indicated by a link with a specific content
    /// </summary>
    /// <param name="http"></param>
    /// <param name="link"></param>
    /// <param name="content"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The raw response, which should be checked for success</returns>
    public static Task<HttpResponseMessage> PutAsync(this HttpClient http, TemplatedLink link, object? content, CancellationToken? cancellationToken = null, IProgress<HttpProgress>? progress = null)
    {
        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Put,
            RequestUri = (Uri)link,
            Content = HttpContentFactoryWinRT.CreateBody(content),
        };

        return SendRequestInternal(http, request, cancellationToken, progress);
    }

    private static async Task<HttpResponseMessage> SendRequestInternal(HttpClient http, HttpRequestMessage request, CancellationToken? cancellationToken, IProgress<HttpProgress>? progress = null)
    {
        try
        {
            return await http.SendRequestAsync(request, HttpCompletionOption.ResponseContentRead).AsTask(cancellationToken ?? CancellationToken.None, progress);
        }
        catch (Exception ex)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            throw NetworkException.CreateFromWinRT(request.RequestUri, request.TransportInformation.ServerCertificate, ex);
        }
    }
}
#endif