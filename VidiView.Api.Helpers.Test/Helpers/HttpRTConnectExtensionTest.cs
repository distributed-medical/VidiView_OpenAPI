using Windows.Web.Http;
using VidiView.Api.Exceptions;

namespace VidiView.Api.Helpers.Test.Helpers;

#if (!GITHUB_ACTION) // Cannot be run without some manual interventions

[TestClass]
public class HttpRTConnectExtensionTest
{
    [TestMethod]
    [DataRow("dc2.ad.vidiview.com", "common name does not match the host name")]
    [DataRow("expired.badssl.com", "certificate has expired")]
    [DataRow("wrong.host.badssl.com", "common name does not match the host name")]
    [DataRow("self-signed.badssl.com", "certificate authority is invalid or incorrect")]
    [DataRow("untrusted-root.badssl.com", "certificate authority is invalid or incorrect")]
    [DataRow("revoked.badssl.com", "certificate has been revoked")]
    [DataRow("dh480.badssl.com", "An error occurred in the secure channel support")]
    [DataRow("dh-small-subgroup.badssl.com", "An error occurred in the secure channel support")]
    [DataRow("sha1-intermediate.badssl.com", "remote certificate is invalid")]
    [DataRow("sha1-2017.badssl.com", "remote certificate is invalid")]
    [DataRow("https://tls-v1-0.badssl.com:1010", "An error occurred in the secure channel support")]

//    [DataRow("no-sct.badssl.com", "TLS Alert", "HandshakeFailure")]
    public async Task VerifyInvalidCertificateException(string hostName, params string[] expectedErrorMessage)
    {
        var http = CreateHttpClient();

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
    [DataRow("non.existent.host.com", "The host name could not be resolved")]
    [DataRow("www.google.com", "Host is not a VidiView Server")]
    public async Task VerifyNonVidiViewServerHostException(string hostName, params string[] expectedErrorMessage)
    {
        var http = CreateHttpClient();

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

    private static HttpClient CreateHttpClient()
    {
        var http = new HttpClient();
        return http;
    }
}
#endif