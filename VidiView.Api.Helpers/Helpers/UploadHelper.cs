using System.Net;
using System.Net.Http.Headers;
using VidiView.Api.DataModel;

namespace VidiView.Api.Helpers;

public class UploadHelper
{
    readonly HttpClient _http;

    public UploadHelper(HttpClient http)
    {
        _http = http;        
    }

    /// <summary>
    /// Upload a file stream to server, with automatic resume for interrupted uploads
    /// </summary>
    /// <param name="link"></param>
    /// <param name="stream"></param>
    /// <param name="contentType"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Image> ResumeUploadAsync(TemplatedLink link, Stream stream, string contentType, CancellationToken cancellationToken)
    {
        // We should start by checking resumability of this request
        var resumePosition = await GetResumePositionAsync(link, stream.Length, contentType, cancellationToken).ConfigureAwait(false);
        stream.Position = resumePosition;

        return await UploadAsync(link, stream, contentType, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Upload a file stream to server, starting at the current stream position
    /// </summary>
    /// <param name="link"></param>
    /// <param name="stream"></param>
    /// <param name="contentType"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Image> UploadAsync(TemplatedLink link, Stream stream, string contentType, CancellationToken cancellationToken)
    {
        var httpContent = HttpContentFactory.CreateBody(stream, contentType);

        // Indicate with a content range header the position we start uploading from
        httpContent.Headers.ContentRange = new ContentRangeHeaderValue(stream.Position, stream.Length - stream.Position - 1, stream.Length);

        var response = await _http.PostAsync(link.ToUrl(), httpContent, cancellationToken).ConfigureAwait(false);
        await response.AssertSuccessAsync().ConfigureAwait(false);

        return response.Deserialize<Image>();
    }

    /// <summary>
    /// Returns the resume position for this upload
    /// </summary>
    /// <param name="link">Destination resource Uri</param>
    /// <param name="streamLength">The total length of the file to upload</param>
    /// <param name="contentType">The content type to upload</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<long> GetResumePositionAsync(TemplatedLink link, long streamLength, string contentType, CancellationToken cancellationToken)
    {
        // Send empty request indicating only file length,
        // to check if we can resume this upload
        var httpContent = HttpContentFactory.CreateBody(new MemoryStream(0), contentType);
        httpContent.Headers.ContentRange = new ContentRangeHeaderValue(streamLength);

        var response = await _http.PostAsync((Uri)link, httpContent, cancellationToken).ConfigureAwait(false);
        if (response.IsSuccessStatusCode)
        {
            // Verify if the response has headers indicating we can resume upload
            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                var header = response.Content.Headers.ContentRange;
                if (header?.Unit.Equals("bytes", StringComparison.OrdinalIgnoreCase) == true
                    && header?.To != null)
                {
                    // The To value is inclusive for some strange reason..
                    return header.To.Value + 1;
                }
            }
        }

        // Start from the beginning (no resume)
        return 0;
    }
}
