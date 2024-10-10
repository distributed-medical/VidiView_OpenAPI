using VidiView.Api.DataModel;

namespace VidiView.Api.Helpers;

public static class ApiHomeExtensions
{
    public static ApiCompatibility CheckApiCompatibility(this ApiHome home)
    {
        if (home == null)
            throw new ArgumentNullException(nameof(home));

        if (Version.TryParse(home.ServerVersion, out var serverVersion))
        {
            if (serverVersion < ApiVersion.MinimumServerVersion)
            {
                return ApiCompatibility.ClientApiNewerThanSupported;
            }
        }

        if (!Version.TryParse(home.ApiVersion, out var serverApiVersion))
            return ApiCompatibility.InvalidResponse;
        if (!Version.TryParse(home.CompatibleApiVersion, out var serverApiCompatibleVersion))
            return ApiCompatibility.InvalidResponse;

        // Check if the server is too old or too new
        if (serverApiVersion < ApiVersion.MinimumApiVersion)
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
}
