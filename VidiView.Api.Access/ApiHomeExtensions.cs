using VidiView.Api.DataModel;
using VidiView.Api.DataModel.Exceptions;

namespace VidiView.Api.Access;

public static class ApiHomeExtensions
{
    static Dictionary<HttpClient, ApiHome> _cache = new();

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

        var response = await http.GetAsync(""); // Utilizes BaseAddress
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

    /// <summary>
    /// Returns true if we are ready to authenticate in the current state
    /// </summary>
    /// <param name="home"></param>
    /// <returns>True if any authentication rel exists among the links</returns>
    public static bool CanAuthenticate(this ApiHome home)
    {
        foreach (var kvp in home.Links)
        {
            if (kvp.Value.Rel?.StartsWith("authenticate-") == true)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Returns true if we are authenticated
    /// </summary>
    /// <param name="home"></param>
    /// <returns></returns>
    public static bool IsAuthenticated(this ApiHome home)
    {
        return home.Links.Exists(Rel.Start) // This must always exist
            && home.Links.Exists(Rel.Departments);
    }

    /// <summary>
    /// Ensures the device is registered
    /// </summary>
    /// <param name="home"></param>
    /// <exception cref="E1007_DeviceNotGrantedAccessException"></exception>
    public static void AssertRegistered(this ApiHome home)
    {
        if (IsAuthenticated(home))
            return; // This infers device registrated

        bool isRegistered = home.Links.Exists(Rel.Start) // This must always exist
            && home.Links.Exists(Rel.ClientDeviceRegistration);

        if (!isRegistered)
            throw new E1007_DeviceNotGrantedAccessException("Device is either not registered or denied access");
    }
}
