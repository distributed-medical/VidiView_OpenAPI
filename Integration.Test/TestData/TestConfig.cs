using System;
using VidiView.Api.Headers;

namespace Integration.Test;
public static partial class TestConfig
{
    public const string ServerHostName = "blundmark1.ad.perspektivgruppen.se";

    public const string Username = "vidiview";
    public const string Password = "vidiview";

    public const string StudyId = "1.2.3.5.638513557112734571";

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
        byte[] systemThumbprint = [0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8 ];

        // This will read properties from a local file with
        // a partial implementation of this class providing these parameters
        var tc = typeof(TestConfig);
        var appIdProperty = tc.GetProperty("AppId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            ?? throw new NotImplementedException("You need to contact Distributed Medical to get an API key");
        var secretKeyProperty = tc.GetProperty("SecretKey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            ?? throw new NotImplementedException("You need to contact Distributed Medical to get an API key");

        return new ApiKeyHeader((Guid)appIdProperty.GetValue(null)!,
            systemThumbprint,
            (byte[])secretKeyProperty.GetValue(null)!);
    }
}
