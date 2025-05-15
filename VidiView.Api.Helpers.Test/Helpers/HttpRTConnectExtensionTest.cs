using VidiView.Api.Exceptions;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace VidiView.Api.Helpers.Test.Helpers;

[TestClass]
public class HttpRTConnectExtensionTest
{
    [TestMethod]
    [DataRow("expired.badssl.com")]
    [DataRow("wrong.host.badssl.com")]
    [DataRow("revoked.badssl.com")]
    [DataRow("sha1-intermediate.badssl.com")]
    [DataRow("sha1-2017.badssl.com")]
    [ExpectedException(typeof(E1403_InvalidCertificateException))]
    public async Task VerifyInvalidCertificateException(string hostName)
    {
        var http = CreateHttpClient();
        var result = await http.ConnectAsync(hostName, CancellationToken.None);
    }

    [TestMethod]
    [DataRow("self-signed.badssl.com")]
    [DataRow("untrusted-root.badssl.com")]
    [ExpectedException(typeof(E1400_ConnectServerException), AllowDerivedTypes = true)] // Since we are using a custom validator, the correct certificate exception is lost
    public async Task VerifyInvalidCertificateException2(string hostName)
    {
        var http = CreateHttpClient();
        var result = await http.ConnectAsync(hostName, CancellationToken.None);
    }

    [TestMethod]
#if (!GITHUB_ACTION) // Cannot be run without some manual interventions
    [DataRow("dc2.ad.vidiview.com")]
#endif
    [DataRow("dh480.badssl.com")]
    [DataRow("dh-small-subgroup.badssl.com")]
    [DataRow("https://tls-v1-0.badssl.com:1010")]
    [ExpectedException(typeof(E1400_ConnectServerException))]
    public async Task VerifyConnectFailException(string hostName)
    {
        var http = CreateHttpClient();
        var result = await http.ConnectAsync(hostName, CancellationToken.None);
    }


    [TestMethod]
    [DataRow("www.google.com")]
    [ExpectedException(typeof(E1402_NoVidiViewServerException))]
    public async Task VerifyNoVidiViewServerException(string hostName)
    {
        var http = CreateHttpClient();

        var result = await http.ConnectAsync(hostName, CancellationToken.None);
    }

    [TestMethod]
    [DataRow("non.existent.host.com")]
    [ExpectedException(typeof(E1400_ConnectServerException))]
    public async Task VerifyHostNotFoundException(string hostName)
    {
        var http = CreateHttpClient();

        var result = await http.ConnectAsync(hostName, CancellationToken.None);
    }

    [TestMethod]
    [DataRow("demo0.vidiview.com")] // Real certificate

#if (!GITHUB_ACTION)
    [DataRow("test1.ad.vidiview.com")] // VidiView License CA certificate
    [DataRow("test2.ad.perspektivgruppen.se")] // VidiView License CA certificate
    [DataRow("https://test2.ad.perspektivgruppen.se/vidiview/api/")] // VidiView License CA certificate
#endif
    public async Task VerifySuccess(string hostName)
    {
        var http = CreateHttpClient();

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
        var http = CreateHttpClient();

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
        var http = CreateHttpClient();

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

    private static HttpClient CreateHttpClient()
    {
        var httpFilter = new HttpBaseProtocolFilter
        {
            AllowAutoRedirect = false,
            AllowUI = false,
            AutomaticDecompression = true,
            CookieUsageBehavior = HttpCookieUsageBehavior.NoCookies
        };
        // Disable caching
        httpFilter.CacheControl.ReadBehavior = HttpCacheReadBehavior.NoCache;
        httpFilter.CacheControl.WriteBehavior = HttpCacheWriteBehavior.NoCache;

        // Legacy certificate support
        httpFilter.AcceptLegacyLicenseCertificate(true);

        var http = new HttpClient(httpFilter);
        return http;
    }
}
