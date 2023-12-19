using VidiView.Api.Helpers;
using VidiView.Api.Headers;

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
    /// Provide an api key
    /// </summary>
    /// <param name="http"></param>
    /// <param name="apikey"></param>
    public static void SetApiKey(this HttpClient http, ApiKeyHeader apikey)
    {
        http.DefaultRequestHeaders.Remove(apikey.Name);
        http.DefaultRequestHeaders.Add(apikey.Name, apikey.Value);
    }

    public static void SetActingRole(this HttpClient http, string id)
    {
        throw new NotImplementedException();
    }
}