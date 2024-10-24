using VidiView.Example.HttpHandlers;
using VidiView.Api.Authentication;
using VidiView.Api.Helpers;

namespace VidiView.Example;

/// <summary>
/// This class demonstrates the process of authenticating with
/// the connected VidiView Server.
/// Ensure that the device is registered and granted access in 
/// Step 1 before continuing with this step
/// </summary>
[TestClass]
public class E2_Authenticate
{
    static HttpClient _http = null!;

    /// <summary>
    /// Connect to VidiView Server and ensure device is registered.
    /// </summary>
    /// <param name="context"></param>
    [ClassInitialize]
    public static async Task ClassInitialize(TestContext context)
    {
        // Write api request/response to debug out
        var handler = new DebugLogHandler(HttpClientHandlerFactory.Create());
        _http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(10) };
        _http.SetApiKey(TestConfig.ApiKey());

        await _http.ConnectAsync(TestConfig.ServerHostName, CancellationToken.None);
        var device = await _http.RegisterDeviceAsync("5.0", "jPhone 28 Pro");

        Assert.IsTrue(device.IsGranted, "You must ensure the device is granted before continuing with this example");
    }

    /// <summary>
    /// Authenticate as current Windows user
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task Authenticate_WindowsSingleSignOn()
    {
        _http.ClearAuthentication(); // Start fresh

        var auth = new WindowsAuthenticator(_http);
        if (!await auth.IsSupportedAsync())
            Assert.Inconclusive("Windows authentication is not enabled on the connected system");

        await auth.AuthenticateAsync();

        var authenticatedUser = auth.User;
        Assert.IsNotNull(authenticatedUser);
    }

    /// <summary>
    /// Authenticate as a specific user
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task Authenticate_UsernameAndPassword()
    {
        _http.ClearAuthentication(); // Start fresh

        var auth = new UsernamePasswordAuthenticator(_http);
        auth.Options = new Api.DataModel.TokenRequest { Lifetime = TimeSpan.FromSeconds(10) };

        if (!await auth.IsSupportedAsync())
            Assert.Inconclusive("Username/password authentication is not enabled on the connected system");

        await auth.AuthenticateAsync(
            TestConfig.Username,
            TestConfig.Password);

        var authenticatedUser = auth.User!;
        Assert.IsNotNull(authenticatedUser);
    }

    /// <summary>
    /// Authenticate using X509 / smart-card
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task Authenticate_X509()
    {
        _http.ClearAuthentication(); // Start fresh

        var auth = new X509Authenticator(_http);
        if (!await auth.IsSupportedAsync())
            Assert.Inconclusive("X509 authentication is not enabled on the connected system");

        // Check if we have any certificate that can be used for authentication
        var certs = await auth.EligibleClientCertificatesAsync();
        if (!certs.Any())
            Assert.Inconclusive("No valid client certificate trusted by the server was found");

        await auth.AuthenticateAsync(certs[0]);

        var authenticatedUser = auth.User;
        Assert.IsNotNull(authenticatedUser);
    }
}