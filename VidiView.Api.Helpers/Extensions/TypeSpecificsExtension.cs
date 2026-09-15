using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace VidiView.Api.DataModel;

/// <summary>
/// Helper class for the MediaFile TypeSpecifics classes to convert them to/from JsonElement
/// </summary>
public static class TypeSpecificsExtension
{
    /// <summary>
    /// Serialize as json
    /// </summary>
    /// <param name="specifics"></param>
    /// <returns></returns>
    public static JsonElement ToJson(this MicrophoneSoundPressureLevelCalibrationSpecifics specifics)
    {
        return JsonSerializer.SerializeToElement(specifics, TypeSpecificsJsonContext.Default.MicrophoneSoundPressureLevelCalibrationSpecifics);
    }

    /// <summary>
    /// Serialize as json
    /// </summary>
    /// <param name="specifics"></param>
    /// <returns></returns>
    public static JsonElement ToJson(this VoiceRecordingSpecifics specifics)
    {
        return JsonSerializer.SerializeToElement(specifics, TypeSpecificsJsonContext.Default.VoiceRecordingSpecifics);
    }

    /// <summary>
    /// Deserialize Json
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="mediaFile"></param>
    /// <returns></returns>
    public static T? TypeSpecificsAs<T>(this MediaFile mediaFile)
    {
        var json = mediaFile?.TypeSpecifics;

        if (json == null)
        {
            return default(T);
        }

        var typeInfo = GetTypeInfo<T>();
        return json.Value.Deserialize(typeInfo);
    }

    static JsonTypeInfo<T> GetTypeInfo<T>()
    {
        if (typeof(T) == typeof(MicrophoneSoundPressureLevelCalibrationSpecifics))
        {
            return (JsonTypeInfo<T>)(object)TypeSpecificsJsonContext.Default.MicrophoneSoundPressureLevelCalibrationSpecifics;
        }

        if (typeof(T) == typeof(VoiceRecordingSpecifics))
        {
            return (JsonTypeInfo<T>)(object)TypeSpecificsJsonContext.Default.VoiceRecordingSpecifics;
        }

        throw new NotSupportedException($"Type specifics type '{typeof(T).FullName}' is not supported.");
    }
}

[JsonSourceGenerationOptions(
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.KebabCaseLower,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(MicrophoneSoundPressureLevelCalibrationSpecifics))]
[JsonSerializable(typeof(VoiceRecordingSpecifics))]
internal partial class TypeSpecificsJsonContext : JsonSerializerContext
{
}
