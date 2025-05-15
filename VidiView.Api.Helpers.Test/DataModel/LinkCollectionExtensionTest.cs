using VidiView.Api.DataModel;

namespace VidiView.Api.Helpers.Test.DataModel;

[TestClass]
public class LinkCollectionExtensionTest
{
    [TestMethod]
    public void VerifyExists()
    {
        var links = new LinkCollection([
                new Link { Rel = "Test1", Href="http://test1/" },
                new Link { Rel = "Test2", Href="http://test2/" }
            ]);

        Assert.IsTrue(links.Exists("Test1"));

        Assert.IsFalse(links.Exists("test1"), "Case sensitive expected");
        Assert.IsFalse(links.Exists("test3"));
    }

    [TestMethod]
    public void VerifyTryGet()
    {
        var links = new LinkCollection([
                new Link { Rel = "Test1", Href="http://test1/" },
                new Link { Rel = "Test2", Href="http://test2/" }
            ]);

        Assert.IsTrue(links.TryGet("Test1", out var link));
        Assert.AreEqual("Test1", link.Rel);

        Assert.IsFalse(links.TryGet("test1", out link), "Case sensitive expected");
    }

    [TestMethod]
    public void VerifyGetRequired()
    {
        var links = new LinkCollection([
                new Link { Rel = "Test1", Href="http://test1/" },
                new Link { Rel = "Test2", Href="http://test2/" }
            ]);

        var link = links.GetRequired("Test1");
        Assert.AreEqual("Test1", link.Rel);

        Assert.Throws<KeyNotFoundException>(() => link = links.GetRequired("test1"), "Case sensitive expected");
    }
}
