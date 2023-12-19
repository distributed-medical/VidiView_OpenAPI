using System.Net.Http.Headers;
using System.Text.Json;

namespace VidiView.Api.Access;

public static class HttpContentFactory
{
    public static JsonSerializerOptions Options { get; set; } = VidiViewJson.DefaultOptions;

    /// <summary>
    /// Create body from stream
    /// </summary>
    /// <param name="content"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    public static HttpContent CreateBody(Stream content, string contentType)
    {
        if (string.IsNullOrEmpty(contentType))
            throw new ArgumentException("The content type must be indicated when content is a stream");

        var result = new StreamContent(content);
        result.Headers.ContentType = new MediaTypeHeaderValue(contentType);

        // The stream might not support length
        try { result.Headers.ContentLength = content.Length; } catch { }

        return result;
    }

    public static HttpContent CreateBody(object? content)
    {
        return CreateBody(content, null);
    }

    public static HttpContent CreateBody(object? content, string? contentType)
    {
        HttpContent result;
        switch (content)
        {
            case null:
                return new ByteArrayContent(Array.Empty<byte>());

            case string str:
                result = new StringContent(str);
                result.Headers.ContentType = new MediaTypeHeaderValue(contentType ?? "text/plain");
                return result;

            case Stream stream:
                throw new NotImplementedException("You should call another overload of this method");

            default:
                var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(content, content.GetType(), Options);
                result = new ByteArrayContent(jsonBytes);
                result.Headers.ContentType = new MediaTypeHeaderValue(contentType ?? "application/json");
                return result;
        }
    }
}