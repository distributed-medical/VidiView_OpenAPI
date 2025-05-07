using System.Net;
using VidiView.Api.Authentication;
using VidiView.Api.DataModel;
using VidiView.Api.Exceptions;
using VidiView.Api.Headers;
using VidiView.Api.Helpers;

namespace VidiView.Example;

/// <summary>
/// This class demonstrates the process of connecting to a VidiView Server.
/// 
/// In order to communicate with VidiView, you will always need to provide
/// the following:
/// 
/// 1. API-key header. 
/// Without this header, the VidiView server will always return an error.
/// The API-key is constructed from an application ID together with a secret hash key
/// The application ID and key is provided by Distributed Medical upon request. 
/// A helper class to create an API-key header value is provided.
/// The API-key is intended to identify the device and application used by 
/// the calling application.
/// 
/// 2. Permission to call the API from the specific device
/// When creating the API-key header, a thumbprint of the calling device must be
/// provided. The server may or may not grant the device access to the server.
/// If device access is not granted, the application user should be informed of
/// this status and instructed to contact a system administrator to be granted access.
/// 
/// </summary>
[TestClass]
public class E1_RegisterDevice
{
    /// <summary>
    /// This is a simple demonstration of providing an API-key header to call the VidiView Server.
    /// For convenience, we have provided a number of simple helper extension methods, that will
    /// provide a robust implementation of calling the API, even handling redirects from reverse-proxies
    /// and pre-authentication etc.
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task HelloVidiView_Explicit()
    {
        var http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };

        // This is for demonstration only. A device unique thumbprint,
        // such as MAC address should be used here..
        byte[] localDeviceThumbprint = [0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8];

        var apiKeyHeader = new ApiKeyHeader(
            TestConfig.ApplicationId,
            localDeviceThumbprint,
            TestConfig.SecretKey);

        http.DefaultRequestHeaders.Add(apiKeyHeader.Name, apiKeyHeader.Value);

        // The API is always located at the following URI:
        var uri = new Uri($"https://{TestConfig.ServerHostName}/vidiview/api/");

        var response = await http.GetAsync(uri);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        // Use the provided helper method to deserialize our
        // data model objects
        var start = await response.DeserializeAsync<ApiHome>();
        
        /* The initial page contains links that must be used to 
           navigate the API. This is very similar to a web page, but
           aimed at an application consumer.
           Please see RFC 8982 for more information about the standard
           that this API adheres to.
           https://datatracker.ietf.org/doc/html/rfc8982
        */

        Assert.IsTrue(start.Links != null && start.Links.Count > 0);
    }

    /// <summary>
    /// This method demonstrates the same flow as above, using our provided helper 
    /// methods. Using this preferred method, you will get support for system
    /// maintenance messages etc.
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task HelloVidiView_Preferred()
    {
        var http = new HttpClient() { Timeout = TimeSpan.FromSeconds(10) };
        http.SetApiKey(TestConfig.ApiKey());

        try
        {
            // When successful, this will store the host name
            // in a dictionary with the supplied HttpClient used as key.
            // As long as the same HttpClient is used, you can always 
            // get the "home page" by calling HomeAsync()
            var result = await http.ConnectAsync(TestConfig.ServerHostName, CancellationToken.None);
            if (result is ConnectionSuccessful)
            {
                var start = await http.HomeAsync();

                /* The initial page contains links that must be used to 
                   navigate the API. This is very similar to a web page, but
                   aimed at an application consumer.
                   Please see RFC 8982 for more information about the standard
                   that this API adheres to.
                   https://datatracker.ietf.org/doc/html/rfc8982
                */

                Assert.IsTrue(start.Links != null && start.Links.Count > 0);
            }
            else
            {
                // The called server is probably behind a reverse proxy
                // and we are supposed to use an external IdP for authentication
                // This more advanced scenario is not implemented in this example
                throw new NotImplementedException();
            }
        }
        catch (E1405_ServiceMaintenanceModeException)
        {
            // The VidiView Server is down for maintenance.
            // The exception has more details on when it is expected to back online etc
            throw;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [TestMethod]
    public async Task RegisterDevice()
    {
        var http = new HttpClient() { Timeout = TimeSpan.FromSeconds(10) };
        http.SetApiKey(TestConfig.ApiKey());
        var result = await http.ConnectAsync(TestConfig.ServerHostName, CancellationToken.None);
        if (result is ConnectionSuccessful success)
        {
            // Ensure this device is registered and granted access.
            // Please provide the application version and device model name
            var device = await http.RegisterDeviceAsync("5.0", "jPhone 28 Pro");

            if (device.IsGranted)
            {
                // This device is granted access, and we may continue with authentication
                return;
            }
            else
            {
                // This device is not granted access. The user must now contact a system
                // administrator and request that the calling device is granted access.
                // The calling device should be identified using the received token
                Assert.IsTrue(device.RegistrationToken?.Length > 0);
                Assert.Fail($"The calling device is not granted access. Token = {device.RegistrationToken}");
            }
        }
    }

}