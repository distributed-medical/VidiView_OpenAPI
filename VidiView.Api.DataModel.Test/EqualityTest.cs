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

    [TestMethod]
    public void ObjectAccessEquals()
    {
        var userId = Guid.NewGuid();
        var departmentId = Guid.NewGuid();

        var a = new ObjectAccess
        {
            UserId = userId,
            Department = new IdAndName(departmentId, "Cardiology"),
            Granted = 7,
            Denied = 2
        };

        var b = new ObjectAccess
        {
            UserId = null,
            Department = new IdAndName(departmentId, null),
            Granted = 7,
            Denied = 2
        };

        var c = new ObjectAccess
        {
            UserId = userId,
            Department = new IdAndName(departmentId, "Cardiology"),
            Granted = 7,
            Denied = 1
        };

        Assert.AreEqual(a, b);
        Assert.AreNotEqual(a, c);
    }
}

