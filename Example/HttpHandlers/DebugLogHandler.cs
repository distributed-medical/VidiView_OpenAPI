using System.Diagnostics;
using System.Text;

namespace VidiView.Example.HttpHandlers;

/// <summary>
/// This handler will log http request/response information to Debug out
/// </summary>
public class DebugLogHandler : DelegatingHandler
{
    public DebugLogHandler(HttpMessageHandler innerHandler) : base(innerHandler)
    {
    }

    public bool WriteRequestBodyToDebugOut { get; set; } = false;

    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var sb = new StringBuilder();
        Stopwatch? sw = null;

        try
        {
            WriteRequestAsync(sb, request).Wait();

            sw = Stopwatch.StartNew();
            var response = base.Send(request, cancellationToken);

            WriteResponse(sb, sw, response);
            return response;
        }
        catch (Exception ex)
        {
            WriteException(sb, sw, ex);
            throw;
        }
        finally
        {
            Debug.WriteLine(sb.ToString());
        }
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var sb = new StringBuilder();
        Stopwatch? sw = null;

        try
        {
            await WriteRequestAsync(sb, request);

            sw = Stopwatch.StartNew();
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            WriteResponse(sb, sw, response);
            return response;
        }
        catch (Exception ex)
        {
            WriteException(sb, sw, ex);
            throw;
        }
        finally
        {
            Debug.WriteLine(sb.ToString());
        }
    }

    async Task WriteRequestAsync(StringBuilder sb, HttpRequestMessage request)
    {
        sb.Append(request.Method);
        sb.Append(" ");
        sb.Append(request.RequestUri?.AbsoluteUri);
        if (request.Headers.Range != null)
            sb.Append($" [Range: {request.Headers.Range}]");

        if (WriteRequestBodyToDebugOut)
        {
            await DebugWriteBodyAsync(request.Content);
        }
    }

    private static async Task DebugWriteBodyAsync(HttpContent? content)
    {
        if (content != null)
        {
            var body = await content.ReadAsStringAsync();
            Debug.WriteLine("BODY:");
            Debug.WriteLine(body);
        }
    }

    void WriteResponse(StringBuilder sb, Stopwatch? sw, HttpResponseMessage response)
    {
        sb.Append($" => {(int)response.StatusCode} {response.ReasonPhrase} after {sw.Elapsed.TotalMilliseconds:0}ms");
        if (response.Headers.Location != null)
            sb.Append($" [Location: {response.Headers.Location}]");
        if (response.Content.Headers.ContentRange != null)
            sb.Append($" [Content-Range: {response.Content.Headers.ContentRange}]");
        if (response.Content.Headers.ContentType != null)
            sb.Append($" [Content-Type: {response.Content.Headers.ContentType}]");
        if (response.Content.Headers.ContentLength != null)
            sb.Append($" [Content-Length: {response.Content.Headers.ContentLength}]");

    }

    static void WriteException(StringBuilder sb, Stopwatch? sw, Exception ex)
    {
        sb.AppendLine("");
        sb.Append($"  Failed to execute operation after {sw?.Elapsed.TotalMilliseconds:0}ms => {ex.Message}");
    }
}
