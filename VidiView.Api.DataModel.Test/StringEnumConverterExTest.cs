using System.Text.Json;
using System.Text.Json.Serialization;

namespace VidiView.Api.DataModel.Test;

[TestClass]
public class StringEnumConverterExTest
{
    /// <summary>
    /// Default serialization options. 
    /// Note! The default JsonStringEnumConverter factory overrides the explicit converters defined on the Enums
    /// </summary>
    static JsonSerializerOptions DefaultOptions
    {
        get
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
            //            options.Converters.Add(new JsonStringEnumConverter());
            return options;
        }
    }

    [TestMethod]
    public void Serialize_To_Named_String()
    {
        var mf = new MediaFile
        {
            Status = MediaFileStatus.Verified
        };

        var json = JsonSerializer.Serialize(mf, DefaultOptions);
        Assert.IsTrue(json.IndexOf("\"Status\":\"Verified\"") > -1);
    }

    [TestMethod]
    public void Serialize_Undefined_To_IntegerString()
    {
        var mf = new MediaFile
        {
            Status = (MediaFileStatus)19939
        };

        var json = JsonSerializer.Serialize(mf, DefaultOptions);
        Assert.IsTrue(json.IndexOf("\"Status\":\"19939\"") > -1);
    }

    [TestMethod]
    public void Serialize_Flags_To_String()
    {
        var mf = new MediaFile
        {
            Flags = ImageFlags.FlipHorizontal | ImageFlags.IsRecovered,
        };

        var json = JsonSerializer.Serialize(mf, DefaultOptions);
        Assert.IsTrue(json.IndexOf("\"Flags\":\"IsRecovered, FlipHorizontal\"") > -1);
    }

    [TestMethod]
    public void Serialize_Undefined_Flags_To_IntegerString()
    {
        var mf = new MediaFile
        {
            Flags = ImageFlags.FlipHorizontal | ImageFlags.IsRecovered | (ImageFlags)0x11,
        };

        var json = JsonSerializer.Serialize(mf, DefaultOptions);
        Assert.IsTrue(json.IndexOf("\"Flags\":\"134221841\"") > -1);
    }




    [TestMethod]
    public void Deserialize_Defined_Name()
    {
        string json = "{\"Status\":\"Verified\"}";

        var mf = JsonSerializer.Deserialize<MediaFile>(json, DefaultOptions);
        Assert.AreEqual(MediaFileStatus.Verified, mf.Status);
    }

    [TestMethod]
    public void Deserialize_Defined_Integer()
    {
        string json = "{\"Status\":20}";

        var mf = JsonSerializer.Deserialize<MediaFile>(json, DefaultOptions);
        Assert.AreEqual(MediaFileStatus.Verified, mf.Status);
    }

    [TestMethod]
    public void Deserialize_Defined_IntegerString()
    {
        string json = "{\"Status\":\"20\"}";

        var mf = JsonSerializer.Deserialize<MediaFile>(json, DefaultOptions);
        Assert.AreEqual(MediaFileStatus.Verified, mf.Status);
    }

    [TestMethod]
    public void Deserialize_Defined_Flags()
    {
        string json = "{\"Flags\":\"FlipHorizontal, IsRecovered\"}";

        var mf = JsonSerializer.Deserialize<MediaFile>(json, DefaultOptions);
        Assert.AreEqual(ImageFlags.FlipHorizontal | ImageFlags.IsRecovered, mf.Flags);
    }

    [TestMethod]
    public void Deserialize_Undefined_Integer()
    {
        string json = "{\"Status\":2100}";

        var mf = JsonSerializer.Deserialize<MediaFile>(json, DefaultOptions);
        Assert.AreEqual(2100, (int)mf.Status);
    }

    [TestMethod]
    public void Deserialize_Undefined_IntegerString()
    {
        string json = "{\"Status\":\"2100\"}";

        var mf = JsonSerializer.Deserialize<MediaFile>(json, DefaultOptions);
        Assert.AreEqual(2100, (int)mf.Status);
    }

    [TestMethod]
    public void Deserialize_Undefined_Flags_IntegerString()
    {
        string json = "{\"Flags\":\"134221841\"}";

        var mf = JsonSerializer.Deserialize<MediaFile>(json, DefaultOptions);
        Assert.AreEqual(134221841L, (long)mf.Flags);
        Assert.IsTrue(mf.Flags.HasFlag(ImageFlags.FlipHorizontal));
        Assert.IsTrue(mf.Flags.HasFlag(ImageFlags.IsRecovered));
        Assert.IsTrue(mf.Flags.HasFlag((ImageFlags)0x11));
    }

    [TestMethod]
    public void Deserialize_Undefined_Name_To_Unknown()
    {
        string json = "{\"Status\":\"NewUndefinedState\"}";

        var mf = JsonSerializer.Deserialize<MediaFile>(json, DefaultOptions);
        Assert.AreEqual(MediaFileStatus.Unknown, mf.Status);
    }

    [TestMethod]
    public void Deserialize_Undefined_Name_To_None()
    {
        string json = "{\"ConferenceType\":\"NewUndefinedState\"}";

        var mf = JsonSerializer.Deserialize<Conference>(json, DefaultOptions);
        Assert.AreEqual(ConferenceType.None, mf.ConferenceType);
    }

    [TestMethod]
    public void Deserialize_Unknown_Flags_Ignored()
    {
        string json = "{\"Flags\":\"FlipHorizontal, IsRecovered, StrangeNewThing\"}";

        var mf = JsonSerializer.Deserialize<MediaFile>(json, DefaultOptions);
        Assert.AreEqual(ImageFlags.FlipHorizontal | ImageFlags.IsRecovered, mf.Flags);
    }




}
