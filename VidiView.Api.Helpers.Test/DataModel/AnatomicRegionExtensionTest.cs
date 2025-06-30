using VidiView.Api.DataModel;

namespace VidiView.Api.Helpers.Test.DataModel;

[TestClass]
public class AnatomicRegionExtensionTest
{
    [TestMethod]
    public void AnatomicRegion_With_Marker_And_Location_ToXml()
    {
        var region = new AnatomicRegion
        {
            MapId = new Guid("a743e2e2d3544d71aa1543b62d943cbf"),
            Snomed = new Snomed("{ 77568009 }"),
            Markers = new[]
            {
                new NormalizedPoint(0.701017796993256, 0.382838577032089)
            },
            Locations = new[] { "77568009" }
        };

        var xml = region.ToXml();
        Assert.AreEqual("<Map map=\"a743e2e2d3544d71aa1543b62d943cbf\" snomed=\"{ 77568009 }\"><Marker x=\"0.701017796993256\" y=\"0.382838577032089\" /><Location>77568009</Location></Map>", xml);
    }

    [TestMethod]
    public void AnatomicRegion_With_Location_And_Snomed_ToXml()
    {
        var region = new AnatomicRegion
        {
            MapId = new Guid("a743e2e2d3544d71aa1543b62d943cbf"),
            Snomed = new Snomed("72696002:272741003=24028007"),
            Locations = new[] { "72696002" }
        };

        var xml = region.ToXml();
        Assert.AreEqual("<Map map=\"a743e2e2d3544d71aa1543b62d943cbf\" snomed=\"72696002:272741003=24028007\"><Location>72696002</Location></Map>", xml);
    }

    [TestMethod]
    public void AnatomicRegion_With_Only_Location_ToXml()
    {
        var region = new AnatomicRegion
        {
            MapId = new Guid("a743e2e2d3544d71aa1543b62d943cbf"),
            Locations = new[] { "72696002" }
        };

        var xml = region.ToXml();
        Assert.AreEqual("<Map map=\"a743e2e2d3544d71aa1543b62d943cbf\"><Location>72696002</Location></Map>", xml);
    }
}
