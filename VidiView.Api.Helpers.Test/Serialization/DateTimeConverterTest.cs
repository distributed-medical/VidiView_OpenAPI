using System.Text.Json;
using VidiView.Api.Serialization;

namespace VidiView.Api.Helpers.Test.Serialization;

[TestClass]
public class DateTimeConverterTest
{
    public record TestClass
    {
        public DateTime Dt { get; set; }
    }

    public DateTimeConverterTest()
    {
        // Run in a culture that would produce different date and time 
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
    }

    [TestMethod]
    public void VerifySerializeMinValue()
    {
        var data = new TestClass { Dt = DateTime.MinValue };

        var json = JsonSerializer.Serialize(data, VidiViewJson.DefaultOptions);

        Assert.AreEqual("{\"dt\":\"\"}", json);
    }

    [TestMethod]
    public void VerifySerializeDateOnly()
    {
        var data = new TestClass { Dt = new DateTime(2024, 01, 02) };

        var json = JsonSerializer.Serialize(data, VidiViewJson.DefaultOptions);
        Assert.AreEqual("{\"dt\":\"2024-01-02\"}", json, "Expect date only without time");
    }

    [TestMethod]
    public void VerifyDeserializeEmpty()
    {
        string json = "{\"dt\":\"\"}";
        var data = JsonSerializer.Deserialize<TestClass>(json, VidiViewJson.DefaultOptions);

        Assert.AreEqual(new TestClass(), data);
    }

    [TestMethod]
    public void VerifyDeserializeDateOnly()
    {
        string json = "{\"dt\":\"2024-01-02\"}";
        var data = JsonSerializer.Deserialize<TestClass>(json, VidiViewJson.DefaultOptions);

        // Whith no explicit time zone specified, the default time zone will be used
        var expected = (DateTime)new DateTime(2024, 01, 02);

        Assert.AreEqual(expected, data.Dt);
    }

    [TestMethod]
    public void VerifyDeserializeDateAndTime()
    {
        string json = "{\"dt\":\"2024-01-02 03:04:05\"}";
        var data = JsonSerializer.Deserialize<TestClass>(json, VidiViewJson.DefaultOptions);

        var expected = new DateTime(2024, 01, 02, 03, 04, 05);

        Assert.AreEqual(expected, data.Dt);
    }
}
