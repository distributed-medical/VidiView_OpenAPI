namespace VidiView.Api.DataModel;

public record ProblemDetails
{
    /// <summary>
    /// Problem details. See RFC-9457 (https://www.rfc-editor.org/rfc/rfc9457.html)
    /// </summary>
    public const string ContentType = "application/problem+json";
    public const string VidiViewExceptionUri = "http://schema.vidiview.com/exception/";
    public const string GenericExceptionUri = "tag:";

    public string Type { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Detail { get; init; } = string.Empty;
    public string? ErrorCode { get; init; }

    /// <summary>
    /// The raw response as a string. Can be used to find more details serialized into the response
    /// </summary>
    public string RawResponse { get; set; } = string.Empty;
}
