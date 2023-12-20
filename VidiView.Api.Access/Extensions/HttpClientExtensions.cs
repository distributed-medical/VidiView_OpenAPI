using VidiView.Api.DataModel;

namespace VidiView.Api.Access;

public static class HttpClientExtensions
{
    static Dictionary<HttpClient, ApiHome> _cache = new ();

    /// <summary>
    /// Helper extension to get the API starting point. The result will 
    /// be cached for subsequent calls on the specific HttpClient
    /// </summary>
    /// <param name="http"></param>
    /// <param name="forceReload">Force a reload, for instance after specifying an authentication header</param>
    /// <returns></returns>
    public static async Task<ApiHome> HomeAsync(this HttpClient http, bool forceReload = false)
    {
        if (!forceReload && _cache.TryGetValue(http, out var home))
            return home;

        HttpResponseMessage result;
        try
        {
            result = await http.GetAsync(""); // Utilizes BaseAddress
        }
        catch (Exception ex)
        {
            throw;
        }
        home = result.AssertSuccess().Deserialize<ApiHome>();

        _cache[http] = home;
        return home;
    }

    /// <summary>
    /// Return the cached ApiHome (or null if no cached document exists)
    /// </summary>
    /// <param name="http"></param>
    /// <returns></returns>
    public static ApiHome? CachedHome(this HttpClient http)
    {
        if (_cache.TryGetValue(http, out var home))
            return home;
        else
            return null;
    }

    /// <summary>
    /// Helper method to get and response, verify success and then deserialize result
    /// </summary>
    /// <typeparam name="T">The type to deserialize</typeparam>
    /// <param name="http"></param>
    /// <param name="link">The link to read</param>
    /// <returns></returns>
    public static async Task<T> GetAsync<T>(this HttpClient http, Link link)
    {
        var result = await http.GetAsync(link.ToUrl());
        return result.AssertSuccess().Deserialize<T>();
    }

    /// <summary>
    /// Clear authentication
    /// </summary>
    /// <param name="http"></param>
    public static void ClearAuthentication(this HttpClient http)
    {
        http.DefaultRequestHeaders.Authorization = null;
        InvalidateHome(http);
    }

    /// <summary>
    /// Invalidate the start page forcing a reload on next attempt
    /// </summary>
    /// <param name="http"></param>
    public static void InvalidateHome(this HttpClient http)
    {
        _cache.Remove(http);
    }
}
