using VidiView.Api.Serialization;

namespace VidiView.Api.Helpers.Test.Serialization;

[TestClass]
public class VidiViewNamingPolicyTest
{
    [TestMethod]
    [DataRow("Id", "id")]
    [DataRow("ServiceHost", "service-host")]
    [DataRow("PatientID", "patientid")]
    [DataRow("Links", "_links")]
    public void VerifyName(string name, string expected)
    {
        var p = new VidiViewNamingPolicy();
        Assert.AreEqual(expected, p.ConvertName(name));
    }
}
