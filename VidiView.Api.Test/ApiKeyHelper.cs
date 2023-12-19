using VidiView.Api.Access.Headers;

namespace VidiView.Api.Test;
internal static class ApiKeyHelper
{
    static readonly Guid UnitTestAppId = new Guid("2C452D0D-A3A9-4DE9-8064-EFBB4A560C09");
    static readonly byte[] UnitTestSecret = new byte[] { 183, 229, 240, 239, 178, 170, 252, 0, 4, 245, 20, 202, 97, 2, 61, 165, 75, 14, 49, 226, 163, 129, 155, 218 };

    public static ApiKeyHeader Create()
    {
        var deviceThumbprint = new Guid("6DF37B1E-33F0-4E83-88F2-23DEB99506D8").ToByteArray();

        return new ApiKeyHeader(UnitTestAppId,
            deviceThumbprint,
            UnitTestSecret);
    }
}
