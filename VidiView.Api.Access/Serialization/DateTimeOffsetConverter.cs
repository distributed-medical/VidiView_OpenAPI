using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VidiView.Api.Access.Serialization;

public class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (string.IsNullOrEmpty(value))
            return DateTimeOffset.MinValue;
        else
            return DateTimeOffset.Parse(value, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        if (value == DateTimeOffset.MinValue)
        {
            // No value
            writer.WriteStringValue("");
        }
        else if (value.TimeOfDay == TimeSpan.Zero)
        {
            // Only write date part
            writer.WriteStringValue(value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        }
        else
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz", CultureInfo.InvariantCulture));
        }
    }
}