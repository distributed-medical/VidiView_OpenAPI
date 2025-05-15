using System.Text.Json;
using VidiView.Api.Serialization;

namespace VidiView.Api.Helpers.Test.Serialization;

[TestClass]
public class DateTimeOffsetConverterTest
{
    public record TestClass
    {
        public DateTimeOffset Dto { get; set; }
    }

    public DateTimeOffsetConverterTest()
    {
        // Run in a culture that would produce different date and time 
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
    }

    [TestMethod]
    public void VerifySerializeMinValue()
    {
        var data = new TestClass { Dto = DateTimeOffset.MinValue };

        var json = JsonSerializer.Serialize(data, VidiViewJson.DefaultOptions);

        Assert.AreEqual("{\"dto\":\"\"}", json);
    }

    [TestMethod]
    public void VerifySerializeDateOnly()
    {
        var data = new TestClass { Dto = new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.FromHours(2)) };

        var json = JsonSerializer.Serialize(data, VidiViewJson.DefaultOptions);

        Assert.AreEqual("{\"dto\":\"2024-01-02\"}", json, "Expect date only without timezone when time part is empty");
    }

    [TestMethod]
    public void VerifySerializeDateAndTime()
    {
        var data = new TestClass { Dto = new DateTimeOffset(2024, 01, 02, 03, 04, 05, TimeSpan.FromHours(2)) };

        var json = JsonSerializer.Serialize(data, VidiViewJson.DefaultOptions);

        Assert.AreEqual("{\"dto\":\"2024-01-02T03:04:05.000\\u002B02:00\"}", json, "Expect date, time and timezone info");
    }


    [TestMethod]
    public void VerifyDeserializeEmpty()
    {
        string json = "{\"dto\":\"\"}";
        var data = JsonSerializer.Deserialize<TestClass>(json, VidiViewJson.DefaultOptions);

        Assert.AreEqual(new TestClass(), data);
    }

    [TestMethod]
    public void VerifyDeserializeDateOnly()
    {
        string json = "{\"dto\":\"2024-01-02\"}";
        var data = JsonSerializer.Deserialize<TestClass>(json, VidiViewJson.DefaultOptions);

        // Whith no explicit time zone specified, the default time zone will be used
        var expected = (DateTimeOffset)new DateTime(2024, 01, 02);

        Assert.AreEqual(expected, data.Dto);
    }

    [TestMethod]
    public void VerifyDeserializeDateAndTime()
    {
        string json = "{\"dto\":\"2024-01-02T03:04:05.000-04:30\"}";
        var data = JsonSerializer.Deserialize<TestClass>(json, VidiViewJson.DefaultOptions);

        // Whith no explicit time zone specified, the default time zone will be used
        var expected = new DateTimeOffset(2024, 01, 02, 03, 04, 05, TimeSpan.FromHours(-4.5));

        Assert.AreEqual(expected, data.Dto);
    }
}
