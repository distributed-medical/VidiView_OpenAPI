using VidiView.Example.HttpHandlers;
using VidiView.Api.Authentication;
using VidiView.Api.Helpers;
using VidiView.Api.DataModel;

namespace VidiView.Example.TestData;
internal static class HttpClientFactory
{
    /// <summary>
    /// Create an Http client which is registered and authenticated
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// Normally, a single HttpClient can be created and kept 
    /// during the application lifetime. In this example, each class
    /// will create its own client
    /// </remarks>
    public static async Task<HttpClient> CreateAsync()
    {
        // Write api request/response to debug out
        var handler = new DebugLogHandler(HttpClientHandlerFactory.Create());
        var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(10) };
        http.SetApiKey(TestConfig.ApiKey());

        await http.ConnectAsync(TestConfig.ServerHostName, CancellationToken.None);
        var device = await http.RegisterDeviceAsync("5.0", "jPhone 28 Pro");

        Assert.IsTrue(device.IsGranted, "You must ensure the device is granted before continuing with this example");

        // Authenticate
        var auth = new UsernamePasswordAuthenticator(http);
        auth.Options = new TokenRequest { Scope = "vidiview-study:read vidiview-study:contribute" };
        await auth.AuthenticateAsync(TestConfig.Username, TestConfig.Password);

        return http;
    }
}
