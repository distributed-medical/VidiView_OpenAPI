using VidiView.Api.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VidiView.Api.Helpers.Test.Extensions;

[TestClass]
public class DateTimeExtensionTest
{
    [TestMethod]
    public void ToDateParameter_DateTime_ReturnsExpectedFormat()
    {
        var dt = new DateTime(2024, 5, 6);
        var result = dt.ToDateParameter();
        Assert.AreEqual("20240506", result);
    }

    [TestMethod]
    public void ToDateParameter_NullableDateTime_ReturnsNull()
    {
        DateTime? dt = null;
        var result = dt.ToDateParameter();
        Assert.IsNull(result);
    }

    [TestMethod]
    public void ToDateParameter_NullableDateTime_ReturnsExpectedFormat()
    {
        DateTime? dt = new DateTime(2023, 12, 31);
        var result = dt.ToDateParameter();
        Assert.AreEqual("20231231", result);
    }

    [TestMethod]
    public void ToDateParameter_DateOnly_ReturnsExpectedFormat()
    {
        var dateOnly = new DateOnly(2022, 1, 2);
        var result = dateOnly.ToDateParameter();
        Assert.AreEqual("20220102", result);
    }

    [TestMethod]
    public void ToDateParameter_NullableDateOnly_ReturnsNull()
    {
        DateOnly? dateOnly = null;
        var result = dateOnly.ToDateParameter();
        Assert.IsNull(result);
    }

    [TestMethod]
    public void ToDateParameter_NullableDateOnly_ReturnsExpectedFormat()
    {
        DateOnly? dateOnly = new DateOnly(2021, 7, 15);
        var result = dateOnly.ToDateParameter();
        Assert.AreEqual("20210715", result);
    }
}
