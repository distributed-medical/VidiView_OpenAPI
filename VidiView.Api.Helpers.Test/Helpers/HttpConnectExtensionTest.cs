using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using VidiView.Api.Exceptions;

namespace VidiView.Api.Helpers.Test.Helpers;

[TestClass]
public class HttpConnectExtensionTest
{
    [TestMethod]
    [DataRow("dc2.ad.vidiview.com")]
    [DataRow("expired.badssl.com")]
    [DataRow("wrong.host.badssl.com")]
    [DataRow("self-signed.badssl.com")]
    [DataRow("untrusted-root.badssl.com")]
    [DataRow("revoked.badssl.com")]
    [DataRow("dh480.badssl.com")]
    [DataRow("dh-small-subgroup.badssl.com")]
    [DataRow("sha1-intermediate.badssl.com")]
    [DataRow("sha1-2017.badssl.com")]

    // Why does the client accept TLS 1.0??
    [DataRow("https://tls-v1-0.badssl.com:1010")]

    [ExpectedException(typeof(E1400_ConnectServerException), AllowDerivedTypes = true)]
    public async Task VerifyInvalidCertificateException(string hostName)
    {
        var http = CreateHttpClient(true);
        var result = await http.ConnectAsync(hostName, CancellationToken.None);
    }

    [TestMethod]
    [DataRow("www.google.com")]
    [ExpectedException(typeof(E1402_NoVidiViewServerException))]
    public async Task VerifyNoVidiViewServerException(string hostName)
    {
        var http = CreateHttpClient(true);

        var result = await http.ConnectAsync(hostName, CancellationToken.None);
    }

    [TestMethod]
    [DataRow("non.existent.host.com")]
    [ExpectedException(typeof(E1400_ConnectServerException))]
    public async Task VerifyHostNotFoundException(string hostName)
    {
        var http = CreateHttpClient(true);

        var result = await http.ConnectAsync(hostName, CancellationToken.None);
    }

    [TestMethod]
    [DataRow("demo0.vidiview.com")]

#if (!GITHUB_ACTION) 
    [DataRow("test1.ad.vidiview.com")]
    [DataRow("test2.ad.perspektivgruppen.se")]
    [DataRow("https://test2.ad.perspektivgruppen.se/vidiview/api/")] // VidiView License CA certificate
#endif
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

#if (!GITHUB_ACTION) // Cannot be run without some manual interventions
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
        catch (E1401_NoResponseFromServerException)
        {
            // This is the expected result
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
#endif

    private static HttpClient CreateHttpClient(bool checkRevocation)
    {
        var handler = new SocketsHttpHandler
        {
            // Allow Windows authentication
            Credentials = CredentialCache.DefaultCredentials,
            PreAuthenticate = true,
            AllowAutoRedirect = false,
            AutomaticDecompression = DecompressionMethods.All,
            UseCookies = false
        };

        handler.AcceptLegacyLicenseCertificate(true);

        var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(10) };
        return http;
    }
}

