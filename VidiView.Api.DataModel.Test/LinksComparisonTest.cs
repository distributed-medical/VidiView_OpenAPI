namespace VidiView.Api.DataModel.Test;

[TestClass]
public class LinksComparisonTest
{
    [TestMethod]
    public void Compare_Different_Instance_With_Equal_Links()
    {
        // Arrange
        var mf1 = new MediaFile
        {
            Status = MediaFileStatus.Verified,
            Links = new LinkCollection()
        };
        ((IDictionary<string, Link>)mf1.Links)[Rel.ActiveSessions] = new Link { Href = "http://test.se/uri/", Templated = false };
        ((IDictionary<string, Link>)mf1.Links)[Rel.Self] = new Link { Href = "http://test.se/uri/1", Templated = true };

        var mf2 = new MediaFile
        {
            Status = MediaFileStatus.Verified,
            Links = new LinkCollection()
        };
        ((IDictionary<string, Link>)mf2.Links)[Rel.ActiveSessions] = new Link { Href = "http://test.se/uri/", Templated = false };
        ((IDictionary<string, Link>)mf2.Links)[Rel.Self] = new Link { Href = "http://test.se/uri/1", Templated = true };

        // Act
        Assert.AreEqual(mf1, mf2);
    }

    [TestMethod]
    public void Compare_Different_Instance_With_Unequal_Links()
    {
        // Arrange
        var mf1 = new MediaFile
        {
            Status = MediaFileStatus.Verified,
            Links = new LinkCollection()
        };
        ((IDictionary<string, Link>)mf1.Links)[Rel.ActiveSessions] = new Link { Href = "http://test.se/uri/", Templated = false };
        ((IDictionary<string, Link>)mf1.Links)[Rel.Self] = new Link { Href = "http://test.se/uri/1", Templated = true };

        var mf2 = new MediaFile
        {
            Status = MediaFileStatus.Verified,
            Links = new LinkCollection()
        };
        ((IDictionary<string, Link>)mf2.Links)[Rel.ActiveSessions] = new Link { Href = "http://test.se/uri/", Templated = false };
        ((IDictionary<string, Link>)mf2.Links)[Rel.Self] = new Link { Href = "http://test.se/uri/2", Templated = true }; // Note URI difference

        // Act
        Assert.AreNotEqual(mf1, mf2);
    }

    [TestMethod]
    public void Compare_With_Null()
    {
        // Arrange
        var mf1 = new MediaFile
        {
            Status = MediaFileStatus.Verified,
            Links = new LinkCollection()
        };
        ((IDictionary<string, Link>)mf1.Links)[Rel.ActiveSessions] = new Link { Href = "http://test.se/uri/", Templated = false };
        ((IDictionary<string, Link>)mf1.Links)[Rel.Self] = new Link { Href = "http://test.se/uri/1", Templated = true };

        var mf2 = new MediaFile
        {
            Status = MediaFileStatus.Verified,
        };

        // Act
        Assert.AreNotEqual(mf1, mf2);
    }
}
