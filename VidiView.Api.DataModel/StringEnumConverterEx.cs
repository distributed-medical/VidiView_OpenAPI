using System.Diagnostics;
using System.Numerics;
using System.Text.Json;

namespace VidiView.Api;

/// <summary>
/// This is a converter that supports replacing undefined Enum values 
/// with the explicit Unknown value if unsupported value arrives
/// </summary>
/// <typeparam name="TEnum"></typeparam>
public class StringEnumConverterEx<TEnum> : JsonConverter<TEnum>
    where TEnum : System.Enum
{
    /// <summary>
    /// Write string representation of enum
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        if (value != null)
        {
            writer.WriteStringValue(value.ToString());
        }
    }

    public override TEnum? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Debug.Assert(typeToConvert == typeof(TEnum));

        string? value;
        try
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    value = reader.GetString();
                    if (Enum.TryParse(typeof(TEnum), value, options.PropertyNameCaseInsensitive, out var result1))
                    {
                        return (TEnum)result1;
                    }
                    break;

                case JsonTokenType.Number:
                    // This will under normal circumstances succeed, even for unknown values!
                    var success = reader.TryGetInt64(out var l);
                    if (success)
                    {
                        value = l.ToString();
                        success = Enum.TryParse(typeof(TEnum), value, out var result2);
                        if (success)
                        {
                            return (TEnum)result2;
                        }
                    }
                    else
                    {
                        goto default;
                    }
                    break;

                default:
                    value = reader.GetString();
                    break;
            }
        }
        catch (Exception ex)
        {
            value = $"[Error: {ex.Message}]";
        }

        Debug.WriteLine($"{typeof(TEnum).FullName} does not define value {value}");

        if (typeof(TEnum).GetCustomAttributes(typeof(FlagsAttribute), false).Any())
        {
            // This is a Flags enum. Parse supported flags
            return ParseKnownFlags(value, options);
        }
        else if (Enum.TryParse(typeof(TEnum), "Unknown", options.PropertyNameCaseInsensitive, out var result3))
        {
            // Enum has Unknown value. Use this
            return (TEnum)result3;
        }
        else if (Enum.TryParse(typeof(TEnum), "None", options.PropertyNameCaseInsensitive, out var result4))
        {
            // Enum has None value. Use this
            return (TEnum)result4;
        }

        throw new NotSupportedException($"{typeof(TEnum).FullName} does not define value {value}. No fallback alternative is defined");
    }

    private static TEnum ParseKnownFlags(string? value, JsonSerializerOptions options)
    {
        // This is a Flags enum. Parse supported flags
        var tp = Enum.GetUnderlyingType(typeof(TEnum));

        if (tp == typeof(long))
        {
            return ParseKnownFlags<long>(value, options);
        }
        else if (tp == typeof(int))
        {
            return ParseKnownFlags<int>(value, options);
        }
        else if (tp == typeof(ulong))
        {
            return ParseKnownFlags<ulong>(value, options);
        }
        else if (tp == typeof(uint))
        {
            return ParseKnownFlags<uint>(value, options);
        }
        else
        {
            Debug.Assert(false, "Underlying type not implemented");
            throw new NotImplementedException($"Parse known flags for enumeration of underlying type {tp.Name} is not implemented");
        }
    }

    private static TEnum ParseKnownFlags<TUnderlyingType>(string? value, JsonSerializerOptions options)
        where TUnderlyingType : struct, IBitwiseOperators<TUnderlyingType, TUnderlyingType, TUnderlyingType>
    {
        TUnderlyingType result = default;
        if (value != null)
        {
            foreach (var flag in value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                if (Enum.TryParse(typeof(TEnum), flag, options.PropertyNameCaseInsensitive, out var val))
                {
                    result |= (TUnderlyingType)val;
                }
            }
        }
        return (TEnum)(object)result;
    }
}