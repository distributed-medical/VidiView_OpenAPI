using System.Diagnostics;
using System.Net.Http;
using VidiView.Api.Authentication;
using VidiView.Api.Headers;
using VidiView.Api.Helpers;

namespace Integration.Test;

/// <summary>
/// This class demonstrates the process of connecting
/// to a VidiView Server, registering the local device
/// and afterward authenticate the user
/// </summary>
[TestClass]
public class ConnectAndAuthenticate
{
    static HttpClient _http = null!;

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        _http = new HttpClient();

        // An API key is always required
        var apiKey = TestConfig.ApiKey();
        _http.DefaultRequestHeaders.Add(
            ApiKeyHeader.HeaderName, 
            apiKey.Value);
    }

    /// <summary>
    /// Connect to VidiView Server and register the client device
    /// This should be done initially on all new connections
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task A100_ConnectAndRegisterDevice()
    {
        // The ConnectAsync extension method will connect to the host,
        // trying to follow redirects and handle situations where the 
        // VidiView Server is located behind a reverse proxy that requires
        // an external IdP authentication before allowing traffic through
        var state = await _http.ConnectAsync(TestConfig.ServerHostName, CancellationToken.None);
        Assert.IsInstanceOfType<ConnectionSuccessful>(state);

        // We should register the client device with the API, indicating the 
        // client application version and the device model name.
        var device = await _http.RegisterDeviceAsync("4.0", "Apple iPhone 15");
        Assert.IsNotNull(device?.DeviceId);

        if (!device.IsGranted)
        {
            // The device is successfully registered, but is not granted
            // access to this system. The error contains a registration token
            // that in a client application should be displayed to the user,
            // which in turn provides it to the system administrator
            throw new Exception($"Device not granted access, registration token: {device.RegistrationToken}");
        }
    }

    /// <summary>
    /// Authenticate as current Windows user
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    [Ignore]
    public async Task A200_AuthenticateWindowsSingleSignOn()
    {
        // Note! This method is run after A100_ which will connect to the API
        var auth = new WindowsAuthenticator(_http);
        if (!await auth.IsSupportedAsync())
            Assert.Inconclusive("Windows authentication is not enabled on the connected system");

        await auth.AuthenticateAsync();
        var authenticatedUser = auth.User!;
    }

    /// <summary>
    /// Authenticate as a specific user
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task A300_AuthenticateUsernamePassword()
    {
        // Note! This method is run after A100_ which will connect to the API
        var auth = new UsernamePasswordAuthenticator(_http);
        if (!await auth.IsSupportedAsync())
            Assert.Inconclusive("Username/password authentication is not enabled on the connected system");

        await auth.AuthenticateAsync(
            TestConfig.Username, 
            TestConfig.Password);

        var authenticatedUser = auth.User!;
        Assert.AreEqual("VidiView Test User", authenticatedUser.Name);
    }

}