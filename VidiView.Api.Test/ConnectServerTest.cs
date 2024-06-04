using System.Net;
using VidiView.Api.Helpers;
using VidiView.Api.DataModel;

namespace VidiView.Api.Test;

[TestClass]
public class ConnectServerTest
{
    const string BaseUrl = "https://test1.ad.vidiview.com/vidiview/api/";

    [TestMethod]
    public async Task Deserialize_Error_Async()
    {
        var client = new HttpClient(ServerValidator.TrustVidiViewAuthorityHandler);

        // This call will fail since we don't have an ApiKey header
        var result = await client.GetAsync(BaseUrl);
        var error = result.Deserialize<ProblemDetails>();

        Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, result.StatusCode);
        Assert.AreEqual("1820", error?.ErrorCode);
        Assert.IsNotNull(error?.Detail);
    }

    [TestMethod]
    public async Task Get_Server_Name_And_Version_Async()
    {
        // Note! For production code, the HttpClient should not be created
        // for every call, and the apikey may be cached for at least an hour
        var apikey = ApiKeyHelper.Create();
        var client = new HttpClient(ServerValidator.TrustVidiViewAuthorityHandler);
        client.DefaultRequestHeaders.Add(apikey.Name, apikey.Value);

        var result = await client.GetAsync(BaseUrl);
        result.EnsureSuccessStatusCode();
        var home = result.Deserialize<ApiHome>();

        Assert.IsNotNull(home);
        Assert.IsNotNull(home.Name);
        Assert.IsNotNull(home.ServerVersion);
    }

    [TestMethod]
    public async Task Register_With_Server_Async()
    {
        var apikey = ApiKeyHelper.Create();
        var client = new HttpClient();
        client.BaseAddress = new Uri(BaseUrl);
        client.DefaultRequestHeaders.Add(apikey.Name, apikey.Value);

        var deviceInfo = new ClientDevice
        {
            AppVersion = "5.0",
            OSVersion = Environment.OSVersion.VersionString,
            Model = "Unit-test",
            DeviceName = Dns.GetHostName(),
        };

        deviceInfo = await DeviceRegistration.RegisterAsync(client, deviceInfo);
        Assert.IsNotNull(deviceInfo);

        if (!deviceInfo.IsGranted)
        {
            throw new Exception($"Device is not granted access. Registration token is {deviceInfo.RegistrationToken}");
        }

        // Device is granted access
    }
}