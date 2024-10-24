using System.Collections.Concurrent;
using System.Net;
using System.Net.Http;
using VidiView.Api.DataModel;
using VidiView.Api.Headers;

namespace VidiView.Api.Helpers;

public static class HttpClientExtensions
{
    static readonly ConcurrentDictionary<HttpClient, ApiHome> _cache = new();
    static readonly ConcurrentDictionary<HttpClient, Uri> _baseAddress = new();

    /// <summary>
    /// Specify the VidiView Server's base address
    /// </summary>
    /// <remarks>This is cached for the http client instance. Used by the ConnectAsync extension when successfully connected</remarks>
    /// <param name="http"></param>
    /// <param name="baseAddress"></param>
    public static void Disconnect(this HttpClient http)
    {
        _baseAddress.TryRemove(http, out _);
        _cache.TryRemove(http, out _);
    }

    /// <summary>
    /// Register client device with server
    /// </summary>
    /// <param name="http"></param>
    /// <param name="appVersion">Application version</param>
    /// <param name="deviceModel">Device model</param>
    /// <returns></returns>
    public static async Task<ClientDevice> RegisterDeviceAsync(this HttpClient http, string appVersion, string? deviceModel)
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
    
    [Obsolete("Only used by Unit-tests")]
    public static void SetBaseAddress(this HttpClient http, Uri baseAddress)
    {
        _baseAddress[http] = baseAddress;
    }

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
            throw new InvalidOperationException("You must call ConnectAsync()");
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
        _cache.TryRemove(http, out _);
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
                http.DefaultRequestHeaders.Add(PseudonymizeHeader.Name, (string?)null);
            }
        }
        else
        {
            http.DefaultRequestHeaders.Remove(PseudonymizeHeader.Name);
        }
    }
}
