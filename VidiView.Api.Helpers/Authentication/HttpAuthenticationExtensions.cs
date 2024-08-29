using VidiView.Api.Helpers;
using VidiView.Api.Headers;
using System.Net.Http;

namespace VidiView.Api.Authentication;

public static class HttpAuthenticationExtensions
{
    /// <summary>
    /// Clear authentication
    /// </summary>
    /// <param name="http"></param>
    public static void ClearAuthentication(this HttpClient http)
    {
        http.DefaultRequestHeaders.Authorization = null;
        http.InvalidateHome();
    }

    /// <summary>
    /// Provide an api key
    /// </summary>
    /// <param name="http"></param>
    /// <param name="applicationId"></param>
    /// <param name="thumbprint"></param>
    /// <param name="key"></param>
    public static void SetApiKey(this HttpClient http, Guid applicationId, byte[] thumbprint, byte[] key)
    {
        SetApiKey(http, new ApiKeyHeader(applicationId, thumbprint, key));
    }

    /// <summary>
    /// Provide an api key, by adding the provided API key
    /// header as a default request header
    /// </summary>
    /// <param name="http"></param>
    /// <param name="apiKey"></param>
    public static void SetApiKey(this HttpClient http, ApiKeyHeader apiKey)
    {
        http.DefaultRequestHeaders.Remove(apiKey.Name);
        http.DefaultRequestHeaders.Add(apiKey.Name, apiKey.Value);
    }

    public static void SetActingRole(this HttpClient http, string id)
    {
        throw new NotImplementedException();
    }
}