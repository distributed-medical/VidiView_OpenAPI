#if WINRT
using System.Runtime.Versioning;
using VidiView.Api.Headers;
using VidiView.Api.DataModel;
using Windows.Web.Http;

namespace VidiView.Api.Helpers;

[SupportedOSPlatform("windows10.0.17763.0")]
public static class HttpClientExtensionsWinRT
{
    static readonly Dictionary<HttpClient, ApiHome> _cache = new();
    static readonly Dictionary<HttpClient, Uri> _baseAddress = new();

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
    /// Register client device with server
    /// </summary>
    /// <param name="http"></param>
    /// <param name="appVersion">Application version</param>
    /// <param name="deviceModel">Device model</param>
    /// <returns></returns>
    public static async Task<ClientDevice> RegisterDeviceAsync(this Windows.Web.Http.HttpClient http, string appVersion, string? deviceModel)
    {
        var api = await http.HomeAsync();
        var device = new ClientDevice
        {
            AppVersion = appVersion,
            OSVersion = Environment.OSVersion.VersionString,
            DeviceName = System.Net.Dns.GetHostName(),
            Model = deviceModel,
        };

        return await DeviceRegistration.RegisterAsync(http, api, device);
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

    [Obsolete("Use the ConnectAsync() extension method instead")]
    public static void SetBaseAddress(this HttpClient http, Uri baseAddress)
    {
        _baseAddress[http] = baseAddress;
        _cache.Remove(http);
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
            throw new InvalidOperationException("You must call ConnectAsync()");

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
            return home!;
        else
            return null;
    }
}
#endif