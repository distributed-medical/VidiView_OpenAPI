using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidiView.Api.DataModel.Test;

[TestClass]
public class EqualityTest
{
    [TestMethod]
    public void IdAndNameTest()
    {
        var g = Guid.NewGuid();

        // Only Id is used in comparison
        Assert.AreEqual(new IdAndName(g, "Test"), new IdAndName(g, "Test"));
        Assert.AreEqual(new IdAndName(g, null), new IdAndName(g, "Test"));
        Assert.AreEqual(new IdAndName(g, null), new IdAndName(g, null));

        Assert.AreNotEqual(new IdAndName(g, "Test"), new IdAndName(Guid.NewGuid(), "Test"));

    }
}

