#if WINRT
using VidiView.Api.Headers;
using VidiView.Api.DataModel;
using Windows.Web.Http;

namespace VidiView.Api.Helpers;

public static class HttpClientExtensionsWinRT
{
    static Dictionary<HttpClient, ApiHome> _cache = new();
    static Dictionary<HttpClient, Uri> _baseAddress = new();

    /// <summary>
    /// Specify the VidiView Server's base address
    /// </summary>
    /// <remarks>This is cached for the http client instance. Used by the ConnectAsync extension when successfully connected</remarks>
    /// <param name="http"></param>
    /// <param name="baseAddress"></param>
    public static void Disconnect(this HttpClient http)
    {
        _baseAddress.Remove(http);
        _cache.Remove(http);
    }

    /// <summary>
    /// Used by unit-test
    /// </summary>
    /// <param name="http"></param>
    /// <param name="baseAddress"></param>
    public static void SetBaseAddress(this HttpClient http, Uri baseAddress)
    {
        _baseAddress[http] = baseAddress;
    }

    /// <summary>
    /// Set base address and home document for this client
    /// </summary>
    /// <param name="http"></param>
    /// <param name="baseAddress"></param>
    /// <param name="apiHome"></param>
    internal static void SetAddressAndHome(this HttpClient http, Uri baseAddress, ApiHome apiHome)
    {
        _baseAddress[http] = baseAddress;
        _cache[http] = apiHome;
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
    /// Enable or disable API level pseudonymization 
    /// </summary>
    /// <param name="http"></param>
    /// <param name="enabled"></param>
    public static void EnablePseudonymization(this HttpClient http, bool enabled)
    {
        if (enabled)
        {
            if (!http.DefaultRequestHeaders.ContainsKey(PseudonymizeHeader.Name))
                {
                    http.DefaultRequestHeaders.Add(PseudonymizeHeader.Name, (string?) null);
            }
        }
        else
        {
            http.DefaultRequestHeaders.Remove(PseudonymizeHeader.Name);
        }
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