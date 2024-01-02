#if WINRT
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using VidiView.Api.Serialization;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace VidiView.Api.Helpers;

internal static class HttpContentFactoryWinRT
{
    public static JsonSerializerOptions Options { get; set; } = VidiViewJson.DefaultOptions;

    public static IHttpContent CreateBody(object? content)
    {
        return CreateBody(content, null);
    }

    public static IHttpContent CreateBody(object? content, string? contentType)
    {
        IHttpContent result;
        switch (content)
        {
            case null:
                return new HttpStringContent(string.Empty);

            case string s:
                result = new HttpStringContent(s);
                result.Headers.ContentType = new HttpMediaTypeHeaderValue(contentType ?? "text/plain");
                result.Headers.ContentLength = (ulong)s.Length;
                return result;

            case IBuffer:
                result = new HttpBufferContent((IBuffer)content);
                result.Headers.ContentType = new HttpMediaTypeHeaderValue(contentType ?? throw new ArgumentException("Content type is required for IBuffer content"));
                result.Headers.ContentLength = ((IBuffer)content).Length;
                return result;

            case IInputStream:
                return CreateBody((IInputStream)content, contentType);

            case Stream:
                // Convert stream to IInputStream and continue
                return CreateBody(((Stream)content).AsRandomAccessStream(), contentType);

            default:
                var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(content, content.GetType(), Options);

                result = new HttpBufferContent(jsonBytes.AsBuffer());
                result.Headers.ContentType = new HttpMediaTypeHeaderValue(contentType ?? "application/json");
                return result;
        }
    }

    /// <summary>
    /// Create body from stream
    /// </summary>
    /// <param name="content"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    public static IHttpContent CreateBody(IInputStream content, string? contentType)
    {
        if (string.IsNullOrEmpty(contentType))
        {
            throw new ArgumentException("The content type must be indicated when content is a stream");
        }

        var result = new HttpStreamContent(content);
        result.Headers.ContentType = new HttpMediaTypeHeaderValue(contentType);

        // Do NOT set a content length here, since it will disable Chunked support for files >4GB

        return result;
    }
}
#endif