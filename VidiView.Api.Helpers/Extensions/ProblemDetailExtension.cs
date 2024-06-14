using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VidiView.Api.DataModel;

public static class ProblemDetailExtension
{
    /// <summary>
    /// Use this helper method to deserialize additional problem properties
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="problem"></param>
    /// <param name="propertyName"></param>
    /// <param name="options"></param>
    /// <returns>The deserialized value or default</returns>
    public static TValue? GetPropertyValue<TValue>(this ProblemDetails? problem, string propertyName, JsonSerializerOptions options)
    {
        if (TryGetPropertyValue<TValue>(problem, propertyName, options, out var value))
            return value;
        else
            return default(TValue);
    }

    /// <summary>
    /// Use this helper method to deserialize additional problem properties
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="problem"></param>
    /// <param name="propertyName"></param>
    /// <param name="options"></param>
    /// <param name="value"></param>
    /// <returns>True if successful</returns>
    public static bool TryGetPropertyValue<TValue>(this ProblemDetails? problem, string propertyName, JsonSerializerOptions options, out TValue? value)
    {
        value = default(TValue);

        if (problem?.RawResponse == null)
            return false;

        var prop = GetJsonProperty(problem, propertyName, options);
        if (prop != null)
        {
            try
            {
                switch (typeof(TValue))
                {
                    case Type type when type == typeof(string):
                        value = (TValue)(object)prop.Value.Value.GetString();
                        return true;

                    case Type type when type == typeof(bool):
                        switch (prop.Value.Value.ValueKind)
                        {
                            case JsonValueKind.False:
                            case JsonValueKind.Null:
                                value = (TValue)(object)false;
                                return true;
                            case JsonValueKind.True:
                                value = (TValue)(object)true;
                                return true;
                            case JsonValueKind.String:
                                switch (prop.Value.Value.GetString())
                                {
                                    case "0":
                                    case "false":
                                        value = (TValue)(object)false;
                                        return true;
                                    case "1":
                                    case "true":
                                        value = (TValue)(object)true;
                                        return true;
                                    default:
                                        throw new ArgumentException("Cannot convert string to bool");
                                }
                            default:
                                throw new ArgumentException("Cannot convert value to bool");
                        }

                    case Type type when type == typeof(int):
                        value = (TValue)(object)prop.Value.Value.GetInt32();
                        return true;

                    case Type type when type == typeof(long):
                        value = (TValue)(object)prop.Value.Value.GetInt64();
                        return true;

                    case Type type when type == typeof(double):
                        value = (TValue)(object)prop.Value.Value.GetDouble();
                        return true;

                    case Type type when type == typeof(float):
                        value = (TValue)(object)prop.Value.Value.GetSingle();
                        return true;

                    case Type type when type == typeof(decimal):
                        value = (TValue)(object)prop.Value.Value.GetDecimal();
                        return true;

                    case Type type when type == typeof(Guid):
                        value = (TValue)(object)prop.Value.Value.GetGuid();
                        return true;

                    default:
                        throw new NotImplementedException("Result value type not implemented");
                }
            }
            catch
            {
            }
        }
        return false;
    }

    private static JsonProperty? GetJsonProperty(ProblemDetails problem, string propertyName, JsonSerializerOptions options)
    {
        bool caseInsensitive = options?.PropertyNameCaseInsensitive ?? false;

        var policy = options?.PropertyNamingPolicy;
        string? convertedPropertyName = policy?.ConvertName(propertyName);

        var stringComparison = caseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        var json = JsonSerializer.Deserialize<JsonElement>(problem.RawResponse);
        foreach (var p in json.EnumerateObject().OfType<JsonProperty>())
        {
            if (p.Name.Equals(propertyName, stringComparison))
            {
                return p;
            }
            else if (convertedPropertyName != null
                && p.Name.Equals(convertedPropertyName, stringComparison))
            {
                return p;
            }
        }

        return null;
    }
}
