using System;
using VidiView.Api.Headers;

namespace VidiView.Example;
public static partial class TestConfig
{
    public const string ServerHostName = "demo0.vidiview.com";

    public static readonly byte[] Thumbprint = [0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8];

    public const string Username = "vidiview";
    public const string Password = "vidiview-demo0";

    public const string StudyId = "1.3.6.1.4.1.29228.100.3715174326.638652902738721452";
    public static readonly Guid DepartmentId = new Guid("a942c6f3637c4972a91e67fd41df84c7");
    public const string PatientId = "196302010001";

    public static readonly Guid ExternalAppId = new Guid("A6FC1192-B803-4200-B64E-7D8403D399EC");
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
