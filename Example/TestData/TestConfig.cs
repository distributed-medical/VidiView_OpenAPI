using System;
using VidiView.Api.Headers;

namespace VidiView.Example;
public static partial class TestConfig
{
    public const string ServerHostName = "blundmark1.ad.perspektivgruppen.se";
    //public const string ServerHostName = "test2.ad.perspektivgruppen.se";
    public static readonly byte[] Thumbprint = [0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8];

    public const string Username = "vidiview";
    public const string Password = "vidiview";

    public const string StudyId = "1.2.3.5.638513557112734571";

    public static Guid ApplicationId
    {
        get
        {
            // This construct is used to read the value from a file that is
            // not part of the public solution
            var tc = typeof(TestConfig);
            var appIdField = tc.GetField("_appId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?? throw new NotImplementedException("You need to contact Distributed Medical to get an API key");
            return (Guid) appIdField.GetValue(null)!;
        }
    }

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
            SecretKey);
    }


}
