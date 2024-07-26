using VidiView.Api.DataModel;

namespace VidiView.Api.Helpers.Test.DataModel;

[TestClass]
public class TemplatedLinkTest
{
    [TestMethod]
    public void Instantiate_With_No_Parameters()
    {
        var tl = new TemplatedLink("https://demo.vidiview.com/");
        Assert.AreEqual(0, tl.Parameters.Count);
        Assert.AreEqual("https://demo.vidiview.com/", tl.ToUrl());
    }

    [TestMethod]
    public void Parse_Parameters()
    {
        var tl = new TemplatedLink("https://demo.vidiview.com/{id}/{?ignore,format}");
        Assert.AreEqual(3, tl.Parameters.Count);
        Assert.AreEqual("id", tl.Parameters[0].Name);
        Assert.IsTrue(tl.Parameters[0].IsPathParam);
        Assert.IsNull(tl.Parameters[0].Value);

        Assert.AreEqual("ignore", tl.Parameters[1].Name);
        Assert.IsFalse(tl.Parameters[1].IsPathParam);
        Assert.IsNull(tl.Parameters[1].Value);

        Assert.AreEqual("format", tl.Parameters[2].Name);
        Assert.IsFalse(tl.Parameters[2].IsPathParam);
        Assert.IsNull(tl.Parameters[2].Value);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Missing_Path_Parameter_Throws()
    {
        var tl = new TemplatedLink("https://demo.vidiview.com/{id}/{?ignore,format}");
        tl.ToUrl();
    }

    [TestMethod]
    public void Missing_Query_Parameters_Removed()
    {
        var tl = new TemplatedLink("https://demo.vidiview.com/{id}/{?ignore,format}");
        tl.Parameters["id"].Value = "2";
        var url = tl.ToUrl();
        Assert.AreEqual("https://demo.vidiview.com/2/", url);
    }

    [TestMethod]
    public void Set_Query_Parameter_Value()
    {
        var tl = new TemplatedLink("https://demo.vidiview.com/{id}/{?ignore,format}");
        tl.Parameters["id"].Value = "2";
        tl.Parameters["format"].Value = "json";
        var url = tl.ToUrl();
        Assert.AreEqual("https://demo.vidiview.com/2/?format=json", url);
    }

    [TestMethod]
    public void Try_Set_Query_Parameter_Value()
    {
        var tl = new TemplatedLink("https://demo.vidiview.com/{id}/{?ignore,format}");
        tl.Parameters["id"].Value = "2";
        tl.Parameters["format"].Value = "json";

        var success = tl.TrySetParameterValue("nonexisting", "hej");
        Assert.IsFalse(success);

        success = tl.TrySetParameterValue("ignore", "hej");
        Assert.IsTrue(success);

        var url = tl.ToUrl();
        Assert.AreEqual("https://demo.vidiview.com/2/?ignore=hej&format=json", url);
    }

    [TestMethod]
    public void Set_Restricted_Character_Encoded()
    {
        var tl = new TemplatedLink("https://demo.vidiview.com/{id}/{?ignore,format}");
        tl.Parameters["id"].Value = "20/10";

        var url = tl.ToUrl();
        Assert.AreEqual("https://demo.vidiview.com/20%2F10/", url);
    }

}
