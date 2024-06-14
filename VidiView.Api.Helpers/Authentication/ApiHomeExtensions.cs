using VidiView.Api.DataModel;
using VidiView.Api.Exceptions;

namespace VidiView.Api.Authentication;

public static class ApiHomeExtensions
{
    /// <summary>
    /// Returns true if we are ready to authenticate in the current state
    /// </summary>
    /// <param name="home"></param>
    /// <returns>True if any authentication rel exists among the links</returns>
    public static bool CanAuthenticate(this ApiHome home)
    {
        if (home?.Links != null)
        {
            foreach (var kvp in home.Links)
            {
                if (kvp.Value.Rel?.StartsWith("authenticate-") == true)
                    return true;
            }
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
            return; // This infers device is registered

        bool isRegistered = home.Links.Exists(Rel.Start) // This must always exist
            && home.Links.Exists(Rel.ClientDeviceRegistration);

        if (!isRegistered)
            throw new E1007_DeviceNotGrantedAccessException("Device is either not registered or denied access");
    }
}
