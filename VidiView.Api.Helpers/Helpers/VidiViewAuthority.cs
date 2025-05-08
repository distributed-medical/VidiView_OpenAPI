using System.Security.Cryptography.X509Certificates;
#if (WINRT)
using Windows.Storage.Streams;
using Windows.Security.Cryptography.Certificates;
using System.Runtime.InteropServices.WindowsRuntime;
#endif

namespace VidiView.Api.Helpers;

public static class VidiViewAuthority
{

    /// <summary>
    /// The public key of the VidiView Authority certificate, that in many
    /// cases will be the root certificate of the VidiView Server's SSL certificate
    /// </summary>
    /// <returns></returns>
    public static X509Certificate2 Certificate()
    {
        var s = string.Join("",
        "MIIFTzCCAzegAwIBAgIQGiooCWlWUr9EvAa05KWP2jANBgkqhkiG9w0BAQsFADBT",
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
        var b = System.Convert.FromBase64String(s);
        return new X509Certificate2(b);
    }

#if (WINRT)
    public static Windows.Security.Cryptography.Certificates.Certificate Certificate2()
    {
        var s = string.Join("",
        "MIIFTzCCAzegAwIBAgIQGiooCWlWUr9EvAa05KWP2jANBgkqhkiG9w0BAQsFADBT",
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

        var b = global::System.Convert.FromBase64String(s);
        var blob = b.AsBuffer();
        return new Certificate(blob);
    }

    public static async Task<bool> IsIssuedByAsync(this Certificate certificate, Certificate issuer)
    {
        var cbp = new ChainBuildingParameters()
        {
            CurrentTimeValidationEnabled = true,
            NetworkRetrievalEnabled = false,
            RevocationCheckEnabled = false,
        };

        // Why is this not the way to do it?
        //cbp.ExclusiveTrustRoots.Add(issuer);

        var list = new List<Certificate>
        {
            issuer
        };

        try
        {
            var chain = await certificate.BuildChainAsync(list, cbp);
            var result = chain.Validate();
            switch (result)
            {
                case ChainValidationResult.Success:
                case ChainValidationResult.Untrusted: // The chain is complete, but the root is not trusted

                    // This feels really strange... 

                    return true;

                case ChainValidationResult.IncompleteChain:
                    // Either we are missing some intermediate cert(s),
                    // or it is not issued by this certificate
                    goto default;

                default:
                    break;
            }
        }
        catch 
        {
        }

        return false;
    }

#endif

}
