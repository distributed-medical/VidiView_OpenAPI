using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VidiView.Api.Serialization;

public class TimeSpanConverter : JsonConverter<TimeSpan>
{
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (string.IsNullOrEmpty(value))
            return TimeSpan.Zero;
        else
            return TimeSpan.Parse(value, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        if (value == TimeSpan.Zero || value == TimeSpan.MinValue)
            writer.WriteNullValue();
        else
            writer.WriteStringValue(value.ToString("c", CultureInfo.InvariantCulture));
    }
}