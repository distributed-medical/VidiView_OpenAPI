#if WINRT
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using Windows.Storage.Streams;

namespace Windows.Security.Cryptography.Certificates;

public static class CertificateExtension
{
    static readonly IBuffer _licenseAuthorityBlob;
    static readonly Certificate _licenseAuthorityCertificate;

    static CertificateExtension()
    {
        // This is the VidiView License Authority certificate in DER format
        var der = string.Join("", "MIIFTzCCAzegAwIBAgIQGiooCWlWUr9EvAa05KWP2jANBgkqhkiG9w0BAQsFADBT",
                                "MSMwIQYDVQQDExpWaWRpVmlldyBMaWNlbnNlIEF1dGhvcml0eTEfMB0GA1UEChMW",
                                "RGlzdHJpYnV0ZWQgTWVkaWNhbCBBQjELMAkGA1UEBhMCU0UwIBcNMTYxMDI1MDcy",
                                "MDEyWhgPMjA3OTEyMzEwMDAwMDBaMFMxIzAhBgNVBAMTGlZpZGlWaWV3IExpY2Vu",
                                "c2UgQXV0aG9yaXR5MR8wHQYDVQQKExZEaXN0cmlidXRlZCBNZWRpY2FsIEFCMQsw",
                                "CQYDVQQGEwJTRTCCAiIwDQYJKoZIhvcNAQEBBQADggIPADCCAgoCggIBANvUTa+J",
                                "NG5L60mCZAAztFoYCUBMGs76Yk/7+a0DCfuNY367uUx8L1Qne6S2ei0awQiEBgrn",
                                "deCSXV7Y8avCdaLZjAv85YLwuYf3J94K2Xj6HDqWsTxJ4H0o6tuL2pmY+cz99AIZ",
                                "A3nPWMF0FfneeM+pxo7sZ4IBhzqaNKSluhjE9ReS1h+230n6/lzvCPv3iw2ksnvK",
                                "TNXKELUGA6kXJzSRPU/vhRDqtdAjJp+YkhpHr5VP3Gz4glUx3mobVOb7u/xJDll6",
                                "f2IIn8O/BrgkatBbEmKcoJOMcmFJclcjBz8ZCfHmK9KQuk73nU+Ijuo5UugBx+cu",
                                "Z6R3GrV2c4fmIcRChCNsO6Hs34uqM94ATzwydzZ4I6Vpk4f6PR8N8rk4ZoDmaQXd",
                                "/s1cARxVH49ts6C201ygQ8wD2ds7GOD9RtoQ2VsNl4Nrzo0p8THPx7tMJyLy2Gjd",
                                "JiMtlQ6AFezeH0BkERmswU+CXyNHR6jfuWcnse0c+KhK4OUkUfb7t61Igfiu8LwP",
                                "yvOtd9/lJZgt0kXcMBvYwaSR2VwSiHXtVf7lcrJr+w17ytZgVCuot2+5SJ8Aa1QE",
                                "FR+KtzYYmtbGU/PvZgNjGwYMpYrPrZikf6xPSQDb9954bB6sj8BOy6tb7+DjkkYK",
                                "8FkjEd2uaM5F+JD0Zv1cqMVlLx+PtYrZxy1LAgMBAAGjHTAbMAwGA1UdEwQFMAMB",
                                "Af8wCwYDVR0PBAQDAgH+MA0GCSqGSIb3DQEBCwUAA4ICAQCEMicj4FxdXn5nItAU",
                                "oJ2oAtOPIMpp3sUnBWtNq0EYViCD7JfjJc1q2BFhgXwSQMqzPZ3onZlryms68ygd",
                                "LMDav2aUjrzfrM82ncX1aZBso6Vdl0GS/HDA38aX5wTTEjpv3LueFbOm3x4vh6WD",
                                "PaWDBssgtEefx6tdzTWK3XOYaHhDw5fnn1k1NNUa3Yx1DJ2HuXxnULJkgdjIDGIZ",
                                "uZ5wduB3OKqiV673OKnKMppPXDLNamVg63gco420sfcqgm3xU0PYDKkPIJ6P1HU3",
                                "A/MdIlLEE+VaHDqvbPcckSQZALsiDrIHqK1vSgbCYdaeEXqCE6EVT1kLiYS6u+TD",
                                "QfNQy+VNpTwyleqpbBvIrEJ5/qYRBs2fq7BPceZG+Pa2PH8svqMypTkB0gLwMY2j",
                                "MLl0ckZkyHD0fcJsKxlAEUNwzcai8z1d2wBHBfDuYsh54+F4PB5bqT031nbTHewj",
                                "QYhW0GvexI1r9qT9O/d+vP21g62DIJ3TVTH5cYPySx4H2vuAW3a+umX1E1jV18FK",
                                "PMDxESuS8WfOCofGkgNVgCfEvXbu730pAT50TY6YDt+a713LDFTQpMyhL+2NRUS+",
                                "emE8nxrrCWlhEhZhqYW7RBTg1yR+Ohja4RESilZ9D4BJNSMxZC2u37Uf8A6kbLny",
                                "kGulQhW18iZk8BtT4sfG+iII4A==");
        var b = global::System.Convert.FromBase64String(der);
        _licenseAuthorityBlob = b.AsBuffer();
        _licenseAuthorityCertificate = new Certificate(_licenseAuthorityBlob);
    }

    /// <summary>
    /// Returns true if the certificate represents the VidiView License Authority
    /// </summary>
    /// <param name="certificate"></param>
    /// <returns></returns>
    public static bool IsVidiViewLicenseAuthority(this Certificate certificate)
    {
        return CryptographicBuffer.Compare(certificate.GetCertificateBlob(), _licenseAuthorityBlob);
    }

    /// <summary>
    /// Returns true if the certificate is issued by the VidiView License authority
    /// </summary>
    /// <param name="certificate"></param>
    /// <returns></returns>
    public static bool IsVidiViewLicenseCertificate(this Certificate certificate)
    {
        var cbp = new ChainBuildingParameters()
        {
            CurrentTimeValidationEnabled = true,
            NetworkRetrievalEnabled = false,
            RevocationCheckEnabled = false,
        };
        cbp.ExclusiveTrustRoots.Add(_licenseAuthorityCertificate);
        var result = ChainValidationResult.OtherErrors;

        // Unfortunately we cannot validate the certificate with our custom root cert:
        // https://github.com/microsoft/WindowsAppSDK/issues/3604
        //var evt = new ManualResetEvent(false);
        //Exception? exc = null;
        //Task.Run(async () => {
        //    try
        //    {
        //        var chain = await certificate.BuildChainAsync(Array.Empty<Certificate>(), cbp);
        //        result = chain.Validate();
        //    }
        //    catch (Exception ex)
        //    {
        //        exc = ex;
        //    }
        //    finally
        //    {
        //        evt.Set();
        //    }
        //});
        //evt.WaitOne();
        //if (exc != null)
        //    throw exc;

        if (certificate.Issuer == _licenseAuthorityCertificate.Subject)
        {
            result = ChainValidationResult.Success;
        }

        return result == ChainValidationResult.Success;
    }

}
#endif