using VidiView.Api.Headers;

namespace VidiView.Api.Helpers.Test.Headers;

[TestClass]
public class ReprDigestHeaderTest
{
    [TestMethod]
    public void Create_Sha256()
    {
        var header = ReprDigestHeader.CreateSha256(new byte[] { 1, 2, 3, 4, 5, 6 });

        Assert.AreEqual("Repr-Digest", header.Name);
        Assert.AreEqual("sha-256", header.Algorithm);
        Assert.AreEqual(":AQIDBAUG:", header.Value);
        Assert.AreEqual("sha-256=:AQIDBAUG:", header.ToString());
    }
}