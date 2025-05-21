using System;
using VidiView.Api.Headers;

namespace VidiView.Api.Helpers.Test.ReverseProxy;
public static partial class TestConfig
{
    public const string ServerHostName = "https://vidiviewtest.regionvarmland.se/login";

    public static readonly byte[] Thumbprint = [0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8];
    
    private static readonly Guid _appId = new Guid("2C452D0D-A3A9-4DE9-8064-EFBB4A560C09");
    private static readonly byte[] _secretKey = new byte[] { 183, 229, 240, 239, 178, 170, 252, 0, 4, 245, 20, 202, 97, 2, 61, 165, 75, 14, 49, 226, 163, 129, 155, 218 };

    public static Guid ApplicationId => _appId;
    public static byte[] SecretKey
    {
        get
        {
            // This construct is used to read the value from a file that is
            // not part of the public solution
            var tc = typeof(TestConfig);
            var appIdField = tc.GetField("_secretKey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?? throw new NotImplementedException("You need to contact Distributed Medical to get an API key");
            return (byte[])appIdField.GetValue(null)!;
        }
    }

    public static ApiKeyHeader ApiKey()
    {
        /* 
         * Note! This API key is for testing purposes only.
         * You need to obtain you own api key to connect to 
         * a production VidiView environment!
         *
         * Please contact Distributed Medical for assistance
         */

        // Generate a system thumbprint of this computer.
        //var systemThumbprint = SystemIdentification.GetSystemIdForPublisher();
        byte[] systemThumbprint = Thumbprint;

        return new ApiKeyHeader(ApplicationId,
            systemThumbprint,
            _secretKey);
    }
}
