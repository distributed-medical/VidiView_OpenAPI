using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using VidiView.Api.DataModel;
using VidiView.Api.Exceptions;

namespace VidiView.Api.Helpers.Test.Helpers;

[TestClass]
public class HttpResponseExtensionTest
{
    [TestMethod]
    public async Task DeserializeHtmlThrows()
    {
        var http = CreateHttpClient(true);

        var response = await http.GetAsync("https://distributedmedical.com");
        await response.AssertNotProblemAsync();

        try
        {
            var study = await response.DeserializeAsync<Study>();
            Assert.Fail("Expected exception");
        }
        catch (E1039_DeserializeException ex)
        {
            Assert.IsTrue(ex.RawResponse.Length > 128);
        }
    }

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
