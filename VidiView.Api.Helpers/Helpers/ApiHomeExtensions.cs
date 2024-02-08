using System.Net.Http;
using VidiView.Api.DataModel;

namespace VidiView.Api.Helpers;

public static class ApiHomeExtensions
{
    static Dictionary<HttpClient, ApiHome> _cache = new();

    /// <summary>
    /// Specify the VidiView Server's host name
    /// </summary>
    /// <param name="http"></param>
    /// <param name="hostName"></param>
    /// <param name="port"></param>
    public static void SetHostName(this HttpClient http, string hostName, int port)
    {
        Uri baseAddress = port == 443 ? new Uri($"https://{hostName}/vidiview/api/") : new Uri($"https://{hostName}:{port}/vidiview/api/");
        http.BaseAddress = baseAddress;
    }

    public static void SetHostName(this HttpClient http, string hostName)
    {
        //if hostName contains :443 it will be omitted in resulting uri  host:443 -> https://host/vidiview/api
        Uri baseAddress = new UriBuilder($"https://{hostName}/vidiview/api/").Uri;
        http.BaseAddress = baseAddress;
    }


    /// <summary>
    /// Helper extension to get the API starting point. The result will 
    /// be cached for subsequent calls on the specific HttpClient
    /// </summary>
    /// <param name="http"></param>
    /// <param name="forceReload">Force a reload, for instance after specifying an authentication header</param>
    /// <returns></returns>
    public static async Task<ApiHome> HomeAsync(this HttpClient http, bool forceReload = false, CancellationToken?  cancellationToken = null)
    {
        if (!forceReload && _cache.TryGetValue(http, out var home))
            return home;

        var response = await http.GetAsync("", cancellationToken ?? CancellationToken.None).ConfigureAwait(false); // Utilizes BaseAddress
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

    public static ApiCompatibility CheckApiCompatibility(this ApiHome home)
    {
        if (home == null)
            throw new ArgumentNullException(nameof(home));

        if (!Version.TryParse(home.ApiVersion, out var serverApiVersion))
            return ApiCompatibility.InvalidResponse;
        if (!Version.TryParse(home.CompatibleApiVersion, out var serverApiCompatibleVersion))
            return ApiCompatibility.InvalidResponse;

        // Check if the server is too old or too new
        if (serverApiVersion < ApiVersion.MinimumServerApiVersion)
            return ApiCompatibility.ClientApiNewerThanSupported;
        if (serverApiCompatibleVersion > ApiVersion.TestedApiVersion) 
            return ApiCompatibility.ClientApiOlderThanSupported;

        // It is probably compatible
        if (serverApiVersion.Major > ApiVersion.TestedApiVersion.Major)
            return ApiCompatibility.ClientApiOldButSupported; // Difference in major version!

        if (serverApiVersion < ApiVersion.TestedApiVersion)
            return ApiCompatibility.ClientApiNewButSupported;

        return ApiCompatibility.UpToDate;
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
