using System.Net;
using System.Net.Http;
using VidiView.Api.Exceptions;

namespace VidiView.Api.Helpers.Test.Helpers;

#if (!GITHUB_ACTION) // Cannot be run without some manual interventions

[TestClass]
public class HttpConnectExtensionTest
{
    [TestMethod]
    [DataRow("dc2.ad.vidiview.com", "remote certificate is invalid", "RemoteCertificateNameMismatch")]
    [DataRow("expired.badssl.com", "remote certificate is invalid", "NotTimeValid")]
    [DataRow("wrong.host.badssl.com", "remote certificate is invalid", "RemoteCertificateNameMismatch")]
    [DataRow("self-signed.badssl.com", "remote certificate is invalid", "UntrustedRoot")]
    [DataRow("untrusted-root.badssl.com", "remote certificate is invalid", "UntrustedRoot")]
    [DataRow("revoked.badssl.com", "remote certificate is invalid", "Revoked")]
    [DataRow("dh480.badssl.com", "TLS Alert", "HandshakeFailure")]
    [DataRow("dh-small-subgroup.badssl.com", "TLS Alert", "HandshakeFailure")]
    [DataRow("sha1-intermediate.badssl.com", "remote certificate is invalid")]
    [DataRow("sha1-2017.badssl.com", "remote certificate is invalid")]

    // Why does the client accept TLS 1.0??
    [DataRow("https://tls-v1-0.badssl.com:1010", "TLS Alert", "HandshakeFailure")]

    //[DataRow("no-sct.badssl.com", "TLS Alert", "HandshakeFailure")]
    public async Task VerifyInvalidCertificateException(string hostName, params string[] expectedErrorMessage)
    {
        var http = CreateHttpClient(true);

        try
        {
            var result = await http.ConnectAsync(hostName, CancellationToken.None);
            Assert.Fail("Expected an exception to be thrown");
        }
        catch (E1002_ConnectException ex)
        {
            if (ex.NotVidiViewServer)
            {
                Assert.Fail("The host certificate was accepted by the client, even though it is invalid!");
            }

            // Verify nice error message
            foreach (var s in expectedErrorMessage)
            {
                StringAssert.Contains(ex.Message, s, StringComparison.InvariantCultureIgnoreCase);
            }
        }
    }

    [TestMethod]
    [DataRow("non.existent.host.com", "no such host")]
    [DataRow("www.google.com", "Host is not a VidiView Server")]
    public async Task VerifyNonVidiViewServerHostException(string hostName, params string[] expectedErrorMessage)
    {
        var http = CreateHttpClient(true);

        try
        {
            var result = await http.ConnectAsync(hostName, CancellationToken.None);
            Assert.Fail("Expected an exception to be thrown");
        }
        catch (E1002_ConnectException ex)
        {
            // Check the Uri parameter
            Assert.AreEqual(hostName, ex.Uri?.Authority);

            // Verify nice error message
            foreach (var s in expectedErrorMessage)
            {
                StringAssert.Contains(ex.Message, s, StringComparison.InvariantCultureIgnoreCase);
            }
        }
    }

    [TestMethod]
    [DataRow("demo0.vidiview.com")]
    public async Task VerifySuccess(string hostName)
    {
        var http = CreateHttpClient(true);

        try
        {
            var result = await http.ConnectAsync(hostName, CancellationToken.None);
        }
        catch (E1820_ApiKeyRequiredException)
        {
            // We have reached a VidiView Server, and this is expected!
            return;
        }
    }

    /// <summary>
    /// Test the response from a server that is not responding.
    /// Please shut down the VidiView Server service and skip setting a maintenance message
    /// </summary>
    /// <param name="hostName"></param>
    /// <param name="expectedErrorMessage"></param>
    /// <returns></returns>
    [TestMethod]
    [DataRow("test1.ad.vidiview.com")]
    public async Task VerifyServerNotResponding(string hostName)
    {
        var http = CreateHttpClient(false);

        try
        {
            var result = await http.ConnectAsync(hostName, CancellationToken.None);
            Assert.Inconclusive("The VidiView Server is responding. Please shut it down");
        }
        catch (E1820_ApiKeyRequiredException)
        {
            // We have reached a VidiView Server
            Assert.Inconclusive("The VidiView Server is responding. Please shut it down");
        }
        catch (E1405_ServiceMaintenanceModeException)
        {
            // We have reached a VidiView Server
            Assert.Inconclusive("The VidiView Server is in maintenance mode. Please shut it down");
        }
        catch (E1421_NoResponseFromServerException)
        {
            // This is the expected result
        }
    }

    /// <summary>
    /// Test the response from a server that is in maintenance mode.
    /// Please shut down the VidiView Server service and specify a maintenance message
    /// </summary>
    /// <param name="hostName"></param>
    /// <param name="expectedErrorMessage"></param>
    /// <returns></returns>
    [TestMethod]
    [DataRow("test1.ad.vidiview.com")]
    public async Task VerifyMaintenanceMode(string hostName)
    {
        var http = CreateHttpClient(false);

        try
        {
            var result = await http.ConnectAsync(hostName, CancellationToken.None);
        }
        catch (E1820_ApiKeyRequiredException)
        {
            // We have reached a VidiView Server
            Assert.Inconclusive("The VidiView Server is responding. Please shut it down");
        }
        catch (E1405_ServiceMaintenanceModeException ex)
        {
            // We have reached a VidiView Server with a maintenance mode message
            Assert.IsTrue(ex.Message?.Length > 1);
        }
    }

    private static HttpClient CreateHttpClient(bool checkRevocation)
    {
        var handler = new HttpClientHandler()
        {
            AllowAutoRedirect = false,
            AutomaticDecompression = DecompressionMethods.Deflate,
            CheckCertificateRevocationList = checkRevocation,
            ClientCertificateOptions = ClientCertificateOption.Manual,
            UseCookies = false,
            UseDefaultCredentials = true,
        };
        var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(30) };
        return http;
    }
}

#endif