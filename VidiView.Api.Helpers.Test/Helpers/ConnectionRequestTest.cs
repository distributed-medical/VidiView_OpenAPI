using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidiView.Api.Helpers.Test.Helpers;

[TestClass]
public class ConnectionRequestTest
{
    [TestMethod]
    public void HostNameOnly()
    {
        var request = new ConnectionRequest("demo.vidiview.com");
        Assert.AreEqual(new Uri("https://demo.vidiview.com"), request.ApiUri);
    }

    [TestMethod]
    public void HostNameAndPort()
    {
        var request = new ConnectionRequest("demo.vidiview.com:777");
        Assert.AreEqual(new Uri("https://demo.vidiview.com:777"), request.ApiUri);
    }

    [TestMethod]
    public void UnsecureHostNameAndPort()
    {
        var request = new ConnectionRequest("http://demo.vidiview.com:80");
        Assert.AreEqual(new Uri("http://demo.vidiview.com:80"), request.ApiUri);
    }

    [TestMethod]
    public void HostNameAndPath()
    {
        var request = new ConnectionRequest("vidiview.region.se/kommun");
        Assert.AreEqual(new Uri("https://vidiview.region.se/kommun"), request.ApiUri);
    }
}
