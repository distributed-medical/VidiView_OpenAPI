using System.Diagnostics;
using System.Text.Json;
using VidiView.Api.Exceptions;

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
            return default;
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
        bool success = TryGetPropertyValue(problem, typeof(TValue), propertyName, options, out var obj);
        value = success ? (TValue?)obj : default;
        return success;
    }

    /// <summary>
    /// Use this helper method to deserialize additional problem properties
    /// </summary>
    /// <param name="problem"></param>
    /// <param name="propertyType"></param>
    /// <param name="propertyName"></param>
    /// <param name="options"></param>
    /// <param name="value"></param>
    /// <returns>True if successful</returns>
    public static bool TryGetPropertyValue(this ProblemDetails? problem, Type propertyType, string propertyName, JsonSerializerOptions options, out object? value)
    {
        value = default;

        try
        {
            if (problem?.RawResponse == null)
                return false;

            var prop = GetJsonProperty(problem, propertyName, options);
            if (prop != null)
            {
                switch (propertyType)
                {
                    case Type type when type == typeof(string):
                        value = prop.Value.Value.GetString();
                        return true;

                    case Type type when type == typeof(bool):
                        switch (prop.Value.Value.ValueKind)
                        {
                            case JsonValueKind.False:
                            case JsonValueKind.Null:
                                value = false;
                                return true;
                            case JsonValueKind.True:
                                value = true;
                                return true;
                            case JsonValueKind.String:
                                switch (prop.Value.Value.GetString())
                                {
                                    case "0":
                                    case "false":
                                        value = false;
                                        return true;
                                    case "1":
                                    case "true":
                                        value = true;
                                        return true;
                                    default:
                                        throw new ArgumentException("Cannot convert string to bool");
                                }
                            default:
                                throw new ArgumentException("Cannot convert value to bool");
                        }

                    case Type type when type == typeof(int):
                        value = prop.Value.Value.GetInt32();
                        return true;

                    case Type type when type == typeof(long):
                        value = prop.Value.Value.GetInt64();
                        return true;

                    case Type type when type == typeof(double):
                        value = prop.Value.Value.GetDouble();
                        return true;

                    case Type type when type == typeof(float):
                        value = prop.Value.Value.GetSingle();
                        return true;

                    case Type type when type == typeof(decimal):
                        value = prop.Value.Value.GetDecimal();
                        return true;

                    case Type type when type == typeof(Guid):
                        value = prop.Value.Value.GetGuid();
                        return true;

                    case Type type when type == typeof(IdAndName):
                        value = prop.Value.Value.Deserialize<IdAndName>(options)!;
                        return true;

                    default:
                        Debug.Assert(false, "Not implemented");
                        return false;
                }
            }
        }
        catch
        {
        }

        return false;
    }

    private static JsonProperty? GetJsonProperty(ProblemDetails problem, string propertyName, JsonSerializerOptions options)
    {
        bool caseInsensitive = options?.PropertyNameCaseInsensitive ?? false;

        var policy = options?.PropertyNamingPolicy;
        string? convertedPropertyName = policy?.ConvertName(propertyName);

        var stringComparison = caseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        JsonElement json;
        try
        {
            json = JsonSerializer.Deserialize<JsonElement>(problem.RawResponse);
        }
        catch (Exception ex)
        {
            throw new E1039_DeserializeException(typeof(JsonElement), ex)
            {
                RawResponse = problem.RawResponse
            };
        }

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
