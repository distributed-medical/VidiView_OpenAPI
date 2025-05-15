using System.Text.Json;
using VidiView.Api.Serialization;

namespace VidiView.Api.Helpers.Test.Serialization;

[TestClass]
public class TimeSpanConverterTest
{
    public record TestClass
    {
        public TimeSpan Ts { get; set; }
    }

    public TimeSpanConverterTest()
    {
        // Run in a culture that would produce different date and time 
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
    }

    [TestMethod]
    public void VerifySerializeMinValue()
    {
        var data = new TestClass { Ts = TimeSpan.MinValue };

        var json = JsonSerializer.Serialize(data, VidiViewJson.DefaultOptions);

        Assert.AreEqual("{\"ts\":null}", json);
    }

    [TestMethod]
    public void VerifySerializeTimeSpan()
    {
        var data = new TestClass { Ts = TimeSpan.FromSeconds(184) };

        var json = JsonSerializer.Serialize(data, VidiViewJson.DefaultOptions);
        Assert.AreEqual("{\"ts\":\"00:03:04\"}", json);
    }

    [TestMethod]
    public void VerifySerializeTimeSpanWithDays()
    {
        var data = new TestClass { Ts = new TimeSpan(1, 2, 3, 4, 123) };

        var json = JsonSerializer.Serialize(data, VidiViewJson.DefaultOptions);
        Assert.AreEqual("{\"ts\":\"1.02:03:04.1230000\"}", json);
    }

    [TestMethod]
    public void VerifyDeserializeEmpty()
    {
        string json = "{\"ts\":\"\"}";
        var data = JsonSerializer.Deserialize<TestClass>(json, VidiViewJson.DefaultOptions);

        Assert.AreEqual(new TestClass(), data);
    }

    [TestMethod]
    public void VerifyDeserializeNull()
    {
        string json = "{\"ts\":null}";
        var data = JsonSerializer.Deserialize<TestClass>(json, VidiViewJson.DefaultOptions);

        Assert.AreEqual(new TestClass(), data);
    }

    [TestMethod]
    public void VerifyDeserializeTimSpanWithDays()
    {
        string json = "{\"ts\":\"1.02:03:04.123\"}";
        var data = JsonSerializer.Deserialize<TestClass>(json, VidiViewJson.DefaultOptions);

        var expected = new TimeSpan(1, 2, 3, 4, 123);

        Assert.AreEqual(expected, data.Ts);
    }

    [TestMethod]
    public void VerifyDeserializeTimeSpan()
    {
        string json = "{\"ts\":\"00:00:05\"}";
        var data = JsonSerializer.Deserialize<TestClass>(json, VidiViewJson.DefaultOptions);

        var expected = TimeSpan.FromSeconds(5);

        Assert.AreEqual(expected, data.Ts);
    }
}
