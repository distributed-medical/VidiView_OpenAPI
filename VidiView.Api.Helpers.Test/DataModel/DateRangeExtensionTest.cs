using VidiView.Api.DataModel;

namespace VidiView.Api.Helpers.Test.DataModel;

[TestClass]
public class DateRangeExtensionTest
{
    [TestMethod]
    public void VerifyIsEmpty()
    {
        DateRange dr = null!;
        Assert.IsTrue(dr.IsEmpty());

        dr = new DateRange();
        Assert.IsTrue(dr.IsEmpty());

        dr = new() { FromDate = DateTime.MinValue };
        Assert.IsTrue(dr.IsEmpty());

        dr = new() { ToDate = new DateTime(2024, 01, 01) };
        Assert.IsFalse(dr.IsEmpty());

        dr = new() { FromDate = new DateTime(2024, 01, 01) };
        Assert.IsFalse(dr.IsEmpty());
    }

    [TestMethod]
    public void VerifyIsSingleDate()
    {
        DateRange dr = null!;
        Assert.IsFalse(dr.IsSingleDate());

        dr = new DateRange();
        Assert.IsFalse(dr.IsSingleDate());

        dr = new() { FromDate = DateTime.MinValue };
        Assert.IsFalse(dr.IsSingleDate());

        dr = new() { FromDate = DateTime.MinValue, ToDate = DateTime.MinValue };
        Assert.IsFalse(dr.IsSingleDate());

        dr = new() { FromDate = new DateTime(2024, 01, 01) };
        Assert.IsFalse(dr.IsSingleDate());

        dr = new() { ToDate = new DateTime(2024, 01, 01) };
        Assert.IsFalse(dr.IsSingleDate());

        dr = new() { FromDate = new DateTime(2024, 01, 01), ToDate = new DateTime(2024, 01, 01) };
        Assert.IsTrue(dr.IsSingleDate());
    }
}
