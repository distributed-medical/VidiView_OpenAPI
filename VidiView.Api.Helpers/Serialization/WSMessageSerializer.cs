﻿using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using VidiView.Api.WSMessaging;

namespace VidiView.Api.Serialization;

public static class WSMessageSerializer
{
    private static readonly ConcurrentDictionary<string, Type> _resolvedTypes = new ConcurrentDictionary<string, Type>();

    /// <summary>
    /// Add external message type that is not enumerated by this library
    /// </summary>
    /// <param name="messageType"></param>
    /// <param name="type"></param>
    /// <exception cref="NotSupportedException"></exception>
    public static void AddMessageType(string messageType, Type type)
    {
        if (!type.IsAssignableTo(typeof(IWSMessage)))
        {
            throw new NotSupportedException($"The specified message type does not implement {typeof(IWSMessage).ToString()}");
        }

        _resolvedTypes[messageType] = type;
    }

    /// <summary>
    /// Serialize message to Json utf-8 byte array
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static byte[] Serialize(this IWSMessage message)
    {
        using var ms = new MemoryStream(16 * 1024);
        JsonSerializer.Serialize(
            new Utf8JsonWriter(ms),
            message,
            message.GetType(),
            VidiViewJson.DefaultOptions);

        return ms.ToArray();
    }

    /// <summary>
    /// Deserialize byte buffer into a message
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static bool TryDeserialize(Span<byte> buffer, out IWSMessage? message)
    {
        return TryDeserialize(buffer, null, out message);
    }

    /// <summary>
    /// Deserialize byte buffer into a message
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="logger"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static bool TryDeserialize(Span<byte> buffer, ILogger? logger, out IWSMessage? message)
    {
        string propertyName = VidiViewJson.DefaultOptions?.PropertyNamingPolicy?.ConvertName(nameof(IWSMessage.MessageType))
            ?? nameof(IWSMessage.MessageType);

        try
        {
            var deserializedModel = JsonSerializer.Deserialize<JsonElement>(buffer);
            if (deserializedModel.TryGetProperty(propertyName, out var messageType))
            {
                var typeName = messageType.GetString();
                if (typeName != null)
                {
                    try
                    {
                        var tp = ResolveMessageType(typeName);
                        message = deserializedModel.Deserialize(tp, VidiViewJson.DefaultOptions) as IWSMessage;
                        return message != null;
                    }
                    catch (NotSupportedException ex)
                    {
                        logger?.LogDebug(ex, "Unsupported message type {type}", typeName);
                    }
                    catch (Exception ex)
                    {
                        logger?.LogWarning(ex, "Failed to deserialize message {type}", typeName);
                    }
                }
            }
            else
            {
                logger?.LogDebug("Cannot deserialize message without MessageType property");
            }
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "Failed to deserialize Json");
        }

        message = null;
        return false;
    }

    /// <summary>
    /// This will look through all loaded assemblies to find a 
    /// type matching the name and that implements IWSMessage
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    private static Type ResolveMessageType(string typeName)
    {
        if (_resolvedTypes.TryGetValue(typeName, out var type))
        {
            return type;
        }

        try
        {
            var typesMatchingName = FindType(typeName);

            // Find type implementing the IWSMessage
            type = typesMatchingName.First();
        }
        catch (Exception ex)
        {
            throw new NotSupportedException($"Unable to resolve type {typeName}. You can add it using AddMessageType() function", ex);
        }

        if (!type.IsAssignableTo(typeof(IWSMessage)))
        {
            throw new NotSupportedException($"The specified message type does not implement {typeof(IWSMessage).ToString()}");
        }

        _resolvedTypes.TryAdd(typeName, type);
        return type;
    }

    private static IEnumerable<Type> FindType(string typeName)
    {
        string[]? genericTypeParams = ExtractGenericTypeParameters(ref typeName);

        if (genericTypeParams == null)
        {
            return FindConcreteType(typeName);
        }
        else
        {
            // Little more work for a generic type
            Type prototype = FindPrototypeType(typeName, genericTypeParams);

            // Recursive lookup for generic type parameters
            Type[] args = new Type[genericTypeParams.Length];
            for (int i = 0; i < genericTypeParams.Length; i++)
            {
                args[i] = FindType(genericTypeParams[i]).FirstOrDefault()
                    ?? throw new TypeLoadException($"Failed to load type {genericTypeParams[i]}");
            }

            return new[] { prototype.MakeGenericType(args) };
        }
    }

    private static Type FindPrototypeType(string typeName, string[] genericTypeParams)
    {
        var allTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes());

        var messageType = from t in allTypes
                          where (t.IsGenericType
                                && t.ToString().StartsWith(typeName + '`' + genericTypeParams.Length.ToString()))
                          select t;

        return messageType.FirstOrDefault()
            ?? throw new TypeLoadException($"Failed to load type {messageType}");
    }

    private static IEnumerable<Type> FindConcreteType(string typeName)
    {
        // Check if the type exists locally, or in Core
        var type = Type.GetType(typeName, false);
        if (type != null)
            return new[] { type };

        // Enumerate all types
        var allTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes());

        return from t in allTypes
               where (t.ToString() == typeName)
               select t;
    }

    /// <summary>
    /// Extract generic type parameter information
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    private static string[]? ExtractGenericTypeParameters(ref string typeName)
    {
        string[]? genericTypeParams = null;
        var genericTypeSeparator = typeName.IndexOf('`');
        if (genericTypeSeparator > -1)
        {
            var s = typeName.Substring(genericTypeSeparator + 1);
            s = s.Substring(s.IndexOf('[') + 1); // Remove parameter count and leading '['
            Debug.Assert(s[s.Length - 1] == ']');
            s = s.Substring(0, s.Length - 1); // Strip last ']'

            genericTypeParams = s.Split(',');
            typeName = typeName.Substring(0, genericTypeSeparator);
        }

        return genericTypeParams;
    }
}