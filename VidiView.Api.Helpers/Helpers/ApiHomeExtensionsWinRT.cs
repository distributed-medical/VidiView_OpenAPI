#if WINRT
using VidiView.Api.DataModel;
using Windows.Web.Http;

namespace VidiView.Api.Helpers;

public static class ApiHomeExtensionsWinRT
{
    static Dictionary<HttpClient, ApiHome> _cache = new();
    static Dictionary<HttpClient, Uri> _baseAddress = new();

    /// <summary>
    /// Specify the VidiView Server's host name
    /// </summary>
    /// <param name="http"></param>
    /// <param name="hostName"></param>
    /// <param name="port"></param>
    public static void SetHostName(this HttpClient http, string hostName, int port = 443)
    {
        Uri baseAddress = port == 443 ? new Uri($"https://{hostName}/vidiview/api/") : new Uri($"https://{hostName}:{port}/vidiview/api/");
        SetBaseAddress(http, baseAddress);
    }

    /// <summary>
    /// Specify the VidiView Server's base address
    /// </summary>
    /// <param name="http"></param>
    /// <param name="baseAddress"></param>
    public static void SetBaseAddress(this HttpClient http, Uri? baseAddress)
    {
        if (baseAddress != null)
            _baseAddress[http] = baseAddress;
        else
            _baseAddress.Remove(http);
    }

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

        if (!_baseAddress.TryGetValue(http, out var uri))
            throw new InvalidOperationException("You must call SetHostName first");

        var response = await http.GetAsync(uri);
        await response.AssertSuccessAsync();
        home = response.Deserialize<ApiHome>();

        _cache[http] = home;
        return home;
    }

    /// <summary>
    /// Invalidate the start page forcing a reload on next attempt
    /// </summary>
    /// <param name="http"></param>
    public static void InvalidateHome(this HttpClient http)
    {
        _cache.Remove(http);
    }

    /// <summary>
    /// Return the cached ApiHome (or null if no cached document exists)
    /// This is used internally
    /// </summary>
    /// <param name="http"></param>
    /// <returns></returns>
    internal static ApiHome? CachedHome(this HttpClient http)
    {
        if (_cache.TryGetValue(http, out var home))
            return home;
        else
            return null;
    }
}
#endif