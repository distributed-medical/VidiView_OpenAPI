using System.Net.Http.Headers;
using VidiView.Api.DataModel;

namespace VidiView.Api.Access;

public static class HttpClientExtensions
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
        if (typeof(T) == typeof(string))
        {
            // Don't deserialize
            return (T)(object)await response.AssertSuccess().Content.ReadAsStringAsync();
        }
        else
        {
            return response.AssertSuccess().Deserialize<T>();
        }
    }

    public static async Task<HttpContentStream> GetStreamAsync(this HttpClient http, Link link, RangeHeaderValue? range = null, CancellationToken? cancellationToken = null)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, link.ToUri());
        request.Headers.Range = range;

        try
        {
            var response = await http.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken ?? CancellationToken.None);
            response.AssertSuccess();
            return await HttpContentStream.CreateFromResponse(http, response);
        }
        catch (Exception ex)
        {
            //if (IsConnectionRefused(ex))
            //    throw new E1404_ServiceUnavailableException(uri, ex);
            throw;
        }
    }

    static bool IsConnectionRefused(Exception ex)
    {
        return (ex.InnerException is System.Net.Sockets.SocketException sexc
            && sexc.SocketErrorCode == System.Net.Sockets.SocketError.ConnectionRefused);
    }

}
