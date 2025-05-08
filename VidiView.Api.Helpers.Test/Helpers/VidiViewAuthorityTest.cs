using Windows.Web.Http;
using VidiView.Api.Exceptions;
using Windows.Web.Http.Filters;
using Windows.Security.Cryptography.Certificates;
using System.Runtime.InteropServices.WindowsRuntime;

namespace VidiView.Api.Helpers.Test.Helpers;

[TestClass]
public class VidiViewAuthorityTest
{
    [TestMethod]
    public async Task VerifyInvalidCertificateException()
    {
        var cert = Test1Certificate();
        Assert.IsFalse( await cert.IsIssuedByAsync(TeliaRootCert()) );
        Assert.IsTrue( await cert.IsIssuedByAsync(VidiViewAuthority.Certificate2() ));
    }

    // Issued by License Authority
    static Certificate Test1Certificate()
    {
        var s = string.Join("",
            "MIIGtDCCBJygAwIBAgIgZivFJja5BLSTgiN089A4FRX3f9MqxWqBBDPcNbs+Y0ow",
            "DQYJKoZIhvcNAQELBQAwUzEjMCEGA1UEAxMaVmlkaVZpZXcgTGljZW5zZSBBdXRo",
            "b3JpdHkxHzAdBgNVBAoTFkRpc3RyaWJ1dGVkIE1lZGljYWwgQUIxCzAJBgNVBAYT",
            "AlNFMB4XDTI1MDUwNzIyMDAwMFoXDTI2MDEzMDIzMDAwMFowLDEbMBkGA1UEAxMS",
            "VmlkaVZpZXcgUjQgU2VydmVyMQ0wCwYDVQQKEwRETUFCMIICIjANBgkqhkiG9w0B",
            "AQEFAAOCAg8AMIICCgKCAgEArepRN3AjfQDrgQehJGnyZIHfO1vo/vKZXKXt3/e9",
            "79AshZDQ3YQ2tyJA6eQb6pUIMe5cQdKy6mSu7rg1ZcPbulUXqGlUNVBom5jdAdWn",
            "17AHRYoI0dAUoCDTSx4pgceEUiZlx7k0HyjCeFwzvstkNSuDkyps561p5YgUFNiD",
            "tc0FSHSKBGcrW7uCRrm6sRC4ETCnXCpO0Riam0KISI/eHZ/C6IveuYXIPD11Gdl4",
            "pcAEX7ILApA2aZoObInMnafczNxAOZSYFBH75ejwuxue/GRMa1mKqMxwez5tFKSF",
            "oQKjYShOUC1zvxJsikJ6fA2emie/Gi2EopZX3CusxVYgR0CsR27+ZzYPzghod6hL",
            "934sR89c6L+p/VQBE/Ky7k6sjkeJqQaH9MoT4y5HDimLIU30/9iSGpkBMtB60d6N",
            "QDY47H1fAO2eOxHUgVdeDGzV84m3Y25IKo16/2GdPaaL1tIgrYuDKVNw5f6beROc",
            "X8tUzbdkS/KXkMvCVm6+kxq/0u3qAS7blY/9oxOnMukwxiliaFyQhmKdleYGakb5",
            "RzQrQA8JZCLQ//p7RB+u5OV5dZLwqEVJgsQvHZmSxoERk4GwKV+XP+LVhV0gTDP5",
            "09UX0xsipipxuNWIIMa6XDvwO46PjbMEk3esp+tKOWFueFMv/mZ3SVk7BQAx3QXX",
            "+9ECAwEAAaOCAZkwggGVMAwGA1UdEwEB/wQCMAAwDgYDVR0PAQH/BAQDAgOoMBsG",
            "CSsGAQQBgeQsAQQOVmlkaVZpZXdTZXJ2ZXIwDgYJKwYBBAGB5CwCBAEqMDkGCSsG",
            "AQQBgeQsAwQsOG9CS3ZOb0htTDNMbkM0KzYxSzRoMnAwKzllbkVkbWhiMXhmTW01",
            "TkxFRT0wDgYKKwYBBAGB5CwDAgQAMBcGCSsGAQQBgeQsBAQKNzkzODUwNjk2NjAg",
            "BgkrBgEEAYHkLAUEE0Rpc3RyaWJ1dGVkIE1lZGljYWwwDQYJKwYBBAGB5CxkBAAw",
            "HQYDVR0lBBYwFAYIKwYBBQUHAwEGCCsGAQUFBwMCMDUGA1UdEQQuMCyCFXRlc3Qx",
            "LmFkLnZpZGl2aWV3LmNvbYITZGMyLmFkLnZpZGl2aWV3LmNvbTASBgorBgEEAYHk",
            "LIN0BAQxMjk2MBIGCisGAQQBgeQsg3cEBDQxMTIwEAYKKwYBBAGB5CyDdQQCNDgw",
            "EgYKKwYBBAGB5CyDdgQENDExMjAPBgkrBgEEAYHkLGkEAjE2MA0GCSqGSIb3DQEB",
            "CwUAA4ICAQAzg21tlp5FDZOwt6e9XdHrByBI5oV0NJ3ir3fJc6rqGLGDiZpdsidi",
            "BSzYbwEw5cGvq37rMoVoOZ9AkfUrL2WO7k4kcJ37lAtNUQL5mSHfpHZavjw3p8GQ",
            "xn9s+UGJnw05sj/CpFdolhHG58n8qV7tr0RnSZYBpHEtkJVnMceNUW5lKwXk1zju",
            "ir3p7vw+pc6SfqUZS1lSxz6U5LJ7IBI4oQ0u1XXG4Ev8ZGMBEntLn4xGrRxZJtDF",
            "J365CU2xmEAeTTsAv87tQr42QtL4o8bE7htNNiZsSFI4Ud3qsko7NTE/F8wSFapU",
            "Szeh0DrBYTR8Pz0uokZlr7RG6ONqdr8l6Rds3mH4YTwECJQVuzJbU+PYl9a2/8kv",
            "buDyiZx1kdsAj39RAlIUXlcqG4k5kytLh8+ZpIYYAYE/mR3wULicVvXRRJOfU+ST",
            "5nNDrk9MbvhyS+mXuTAhjbUzlXIeBZiyscdKGmp8oGHuio63nRAlM9Ao+0oWABBk",
            "jvfgfzj+PKSciFEz46fl4VvWMVucyxU2jh/FUCfvaLIuFWnOxx3ro0uDqJnyCzeZ",
            "2riLLmPTSgaRO3orGMp+o0fngI6JDpQhvDaTaEgFSX+1w3OzlzrlNVpCEK9WMuqq",
            "J55mINjBeqGngR83hAUkZD3DHwS4mnNw21s3egRtDf6JF3MDL50YIg==");

        var b = global::System.Convert.FromBase64String(s);
        var blob = b.AsBuffer();
        return new Certificate(blob);
    }

    static Certificate TeliaRootCert()
    {
        var s = string.Join("",
            "MIIFODCCAyCgAwIBAgIRAJW+FqD3LkbxezmCcvqLzZYwDQYJKoZIhvcNAQEFBQAw",
            "NzEUMBIGA1UECgwLVGVsaWFTb25lcmExHzAdBgNVBAMMFlRlbGlhU29uZXJhIFJv",
            "b3QgQ0EgdjEwHhcNMDcxMDE4MTIwMDUwWhcNMzIxMDE4MTIwMDUwWjA3MRQwEgYD",
            "VQQKDAtUZWxpYVNvbmVyYTEfMB0GA1UEAwwWVGVsaWFTb25lcmEgUm9vdCBDQSB2",
            "MTCCAiIwDQYJKoZIhvcNAQEBBQADggIPADCCAgoCggIBAMK+6yfwIaPzaSZVfp3F",
            "VRaRXP3vIb9TgHot0pGMYzHw7CTww6XScnwQbfQ3t+XmfHnqjLWCi65ItqwA3GV1",
            "7CpNX8GH9SBlK4GoRz6JI5UwFpB/6FcHSOcZrr9FZ7E3GwYq/t75rH2D+1665I+X",
            "Z75Ljo1kB1c4VWk0Nj0TSO9P4tNmHqTPGrdeNjPUtAa9GAH9d4RQAEX1jF3oI7x+",
            "/jXh7VB7qTCNGdMJjmhnXb88lxhTuylixcpecsHHltTbLaC0H2kD7OriUPEMPPCs",
            "81Mt8Bz17Ww5OXOAFshSsCPN4D7c3TxHoLs1iuKYaIu+5b9y7tL6pe0S7fyYGKkm",
            "dtwoSxAgHNN/Fnct7W+A90m7UwW7XWjH1Mh1Fj+JWov3F0fUTPHSiXk+TT2YqGHe",
            "Oh7S+F4D4MHJHIzTjU3TlTazN19jY5szFPAtJmtTfImMMsJu7D0hADnJoWjiUIMu",
            "sDor8zagrC/kb2HCUQk5PotTubtn2txTuXZZNp1D5SDgPTJghSJRt8czu90VL6R4",
            "pgd7gUY2BIbdeTXHlSw7sKMXNeVzH7RcWe/a6hBle3rQf5+ztCo3O3CLm1u5K7fs",
            "slESl1MpWtTwEhDcTwK7EpIvYtQ/aUN8Ddb8WHUBiJ1YFkveupD/RwGJBmr2X7KQ",
            "arMCpgKIv7NHfirZ1fpoeDVNAgMBAAGjPzA9MA8GA1UdEwEB/wQFMAMBAf8wCwYD",
            "VR0PBAQDAgEGMB0GA1UdDgQWBBTwj1k4ALP1j5qWDNXr+nuqF+gTEjANBgkqhkiG",
            "9w0BAQUFAAOCAgEAvuRcYk4k9AwI//DTDGjkk0kiP0Qnb7tt3oNmzqjMDfz1mgbl",
            "dxSR651Be5kqhOX//CHBXfDkH1e3damhXwIm/9fH907eT/j3HEbAek9ALCI18Bmx",
            "0GtnLLCo4MBANzX2hFxc469CeP6nyQ1Q6g2EdvZR74NTxnr/DlZJLo961gzmJ1Tj",
            "TQpgcmLNkQfWpb/ImWvtxBnmq0wROMVvMeJuScg/doAmAyYp4Db29iBT4xdwNBed",
            "Y2gea+zDTYa4EzAvXUYNR0PVG6pZDrlcjQZIrXSHX8f8MVRBE+LHIQ6e4B4N4cB7",
            "Q4WQxYpYxmUKeFfyxiMPAdkgS94P+5KFdSpcc41teyWRyu5FrgZLAMzTsVlQ2jqI",
            "OylDRl6XK1TOU2+NSueW+r9xDkKLfP0ooNBIytrEgUy7onOTJsjrDNYmiLbAJM+7",
            "vVvrdX3pCI6GMyx5dwlppYn8s3CQh3aP0yK7Qs69cwsgJirQmz1wHiRszYd2qReW",
            "t88NkvuOGKmYSdGe/mBEciG5Ge3C9THxOUiIkCR1VBatzvT4aRRkOfujuLpwQMcn",
            "HL/EVlP6Y2XQ8xwOFvVrhlhNGNTkDY6lnVuR3HYkUD/GKvvZt5y11ubQ2egZixVx",
            "SK236thZiNSQvxaz2emsWWFUyBy6ysHK4bkgTI86k4mloMy/0/Z1pHWWbVY=");

        var b = global::System.Convert.FromBase64String(s);
        var blob = b.AsBuffer();
        return new Certificate(blob);
    }


}
