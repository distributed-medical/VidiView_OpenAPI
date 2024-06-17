using System;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace VidiView.Api.WSMessaging;
public class MessageSerializer
{
    /// <summary>
    /// Options to use for Json serialization/deserialization
    /// </summary>
    public static JsonSerializerOptions? JsonSerializerOptions { get; set; }

    /// <summary>
    /// Serialize message to UTF8 byte array
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static byte[] Serialize(WSMessage message)
    {
        using var ms = new MemoryStream(16 * 1024);
        JsonSerializer.Serialize(
            new Utf8JsonWriter(ms),
            message,
            message.GetType(),
            JsonSerializerOptions);

        return ms.ToArray();
    }

    /// <summary>
    /// Deserialize byte buffer into a message
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static bool TryDeserialize(Span<byte> buffer, out WSMessage? message)
    {
        string propertyName = JsonSerializerOptions?.PropertyNamingPolicy?.ConvertName(nameof(WSMessage.MessageType)) 
            ?? nameof(WSMessage.MessageType);

        try
        {
            var deserializedModel = JsonSerializer.Deserialize<JsonElement>(buffer);
            if (deserializedModel.TryGetProperty(propertyName, out var messageType))
            {
                string typeName = $"{typeof(WSMessage).Namespace}.{messageType.GetString()}";

                var tp = Assembly.GetAssembly(typeof(WSMessage))?.GetType(typeName);
                if (tp == null)
                    throw new NotSupportedException($"Type {typeName} is not supported");

                message = JsonSerializer.Deserialize(deserializedModel, tp, JsonSerializerOptions) as WSMessage;
                return message != null;
            }
        }
        catch
        { }

        message = null;
        return false;
    }
}
