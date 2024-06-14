using VidiView.Api.Headers;
using System.Net.Http;
using VidiView.Api.DataModel;
using System.Net;
using System.Collections.ObjectModel;

namespace VidiView.Api.Helpers;

public static class HttpClientExtensions
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
    public static async Task<ClientDevice> RegisterDeviceAsync(this System.Net.Http.HttpClient http, string appVersion, string? deviceModel)
    {
        var api = await http.HomeAsync();
        var device = new ClientDevice
        {
            AppVersion = appVersion,
            OSVersion = Environment.OSVersion.VersionString,
            DeviceName = Dns.GetHostName(),
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

#if (DEBUG)
    // Only use for unit-testing
    public static void SetBaseAddress(this HttpClient http, Uri baseAddress)
    {
        _baseAddress[http] = baseAddress;
    }
#endif


    /// <summary>
    /// Helper extension to get the API starting point. The result will 
    /// be cached for subsequent calls on the specific HttpClient
    /// </summary>
    /// <param name="http"></param>
    /// <param name="forceReload">Force a reload, for instance after specifying an authentication header</param>
    /// <returns></returns>
    public static async Task<ApiHome> HomeAsync(this HttpClient http, bool forceReload = false, CancellationToken? cancellationToken = null)
    {
        if (!forceReload && _cache.TryGetValue(http, out var home))
            return home;

        if (!_baseAddress.TryGetValue(http, out var uri))
        {
            throw new InvalidOperationException("You must call ConnectAsync() extension");
        }

        var response = await http.GetAsync(uri, cancellationToken ?? CancellationToken.None).ConfigureAwait(false);
        await response.AssertSuccessAsync().ConfigureAwait(false);
        home = await response.DeserializeAsync<ApiHome>().ConfigureAwait(false);

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
            if (!http.DefaultRequestHeaders.Contains(PseudonymizeHeader.Name))
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
