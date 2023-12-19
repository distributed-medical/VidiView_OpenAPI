using System.Text.Json;
using System.Text.Json.Serialization;

namespace VidiView.Api.Serialization;

public static class VidiViewJson
{
    static JsonSerializerOptions? _default;

    /// <summary>
    /// Default options for deserializing Json response returned
    /// by a VidiView Server to the DataModel objects
    /// </summary>
    /// <returns></returns>
    public static JsonSerializerOptions DefaultOptions
    {
        get
        {
            if (_default == null)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = new VidiViewNamingPolicy(),
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                };
                options.Converters.Add(new DateTimeConverter());
                options.Converters.Add(new DateTimeOffsetConverter());
                options.Converters.Add(new TimeSpanConverter());
                options.Converters.Add(new JsonStringEnumConverter());
                _default = options;
            }

            return _default;
        }
    }
}
