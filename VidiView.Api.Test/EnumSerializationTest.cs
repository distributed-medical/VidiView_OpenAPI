using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VidiView.Api.DataModel;

namespace VidiView.Api.Test;

[TestClass]
public class EnumSerializationTest
{
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
    public void Serialize_Enum_Value()
    {
        var mf = new MediaFile
        {
            Status = MediaFileStatus.Verified
        };

        var json = JsonSerializer.Serialize(mf, DefaultOptions);
        Assert.IsTrue(json.IndexOf("\"Status\":\"Verified\"") > -1);
    }

    [TestMethod]
    public void Deserialize_Enum_ValidName()
    {
        string json = "{\"Status\":\"Verified\"}";

        var mf = JsonSerializer.Deserialize<MediaFile>(json, DefaultOptions);
        Assert.AreEqual(MediaFileStatus.Verified, mf.Status);
    }

    [TestMethod]
    public void Deserialize_Enum_ValidInteger()
    {
        string json = "{\"Status\":20}";

        var mf = JsonSerializer.Deserialize<MediaFile>(json, DefaultOptions);
        Assert.AreEqual(MediaFileStatus.Verified, mf.Status);
    }

    [TestMethod]
    public void Deserialize_Enum_UnknownInteger()
    {
        string json = "{\"Status\":2100}";

        var mf = JsonSerializer.Deserialize<MediaFile>(json, DefaultOptions);
        Assert.AreEqual(2100, (int)mf.Status);
    }

    [TestMethod]
    public void Deserialize_Unknown_Value()
    {
        string json = "{\"Status\":\"NewUndefinedState\"}";

        var mf = JsonSerializer.Deserialize<MediaFile>(json, DefaultOptions);
        Assert.AreEqual(MediaFileStatus.Unknown, mf.Status);
    }

    [TestMethod]
    public void Serialize_Flags_Value()
    {
        var mf = new MediaFile
        {
            Flags = ImageFlags.FlipHorizontal | ImageFlags.IsRecovered,
        };

        var json = JsonSerializer.Serialize(mf, DefaultOptions);
        Assert.IsTrue(json.IndexOf("\"Flags\":\"IsRecovered, FlipHorizontal\"") > -1);
    }

    [TestMethod]
    public void Deserialize_Flags_Value()
    {
        string json = "{\"Flags\":\"FlipHorizontal, IsRecovered\"}";

        var mf = JsonSerializer.Deserialize<MediaFile>(json, DefaultOptions);
        Assert.AreEqual(ImageFlags.FlipHorizontal | ImageFlags.IsRecovered, mf.Flags);
    }

    [TestMethod]
    public void Deserialize_Unknown_Flags_Skipped()
    {
        string json = "{\"Flags\":\"FlipHorizontal, IsRecovered, StrangeNewThing\"}";

        var mf = JsonSerializer.Deserialize<MediaFile>(json, DefaultOptions);
        Assert.AreEqual(ImageFlags.FlipHorizontal | ImageFlags.IsRecovered, mf.Flags);
    }
}
