using VidiView.Api.Headers;

namespace VidiView.Api.Helpers.Test.Headers;

[TestClass]
public class ApiKeyHeaderTest
{
    [TestMethod]
    public void Create_Api_Key()
    {
        var guid = new Guid("{07EC63E4-199C-4EB7-BAFC-8BB1561858D1}");
        var thumbprint = new byte[] { 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110 };
        var secret = new byte[] { 200, 201, 202, 203, 204, 205, 206, 207, 208, 0, 0, 0, 0 };
        var time = new DateTimeOffset(2024, 02, 03, 09, 15, 35, TimeSpan.FromHours(2));
        var header = new ApiKeyHeader(guid, thumbprint, time.ToUnixTimeSeconds(), secret);

        Assert.AreEqual("X-Api-key", header.Name);
        Assert.AreEqual(guid, header.AppId);
        Assert.AreEqual(time, header.InstanceTimeAsDateTime);
        Assert.AreEqual("5GPsB5wZt066/IuxVhhY0QtkZWZnaGlqa2xtbgAAAABlvegXXDa135RKO4CDlZTFvCqBU6V0sauA2V7mdmluQhqB6ns=", header.Value);
    }
}