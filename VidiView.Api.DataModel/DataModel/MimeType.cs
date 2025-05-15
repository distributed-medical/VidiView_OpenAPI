namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record MimeType
{
    [JsonPropertyName("mime-type")]
    public string Type { get; init; } = string.Empty;

    public string Extension { get; init; } = string.Empty;

    public string[]? AltExtensions { get; init; }

    public string? Description { get; init; }

    public bool AllowUpload { get; init; }

    public override string ToString() => Type;


    [JsonIgnore]
    public bool IsVideoType => Type?.StartsWith("video/") == true;

    [JsonIgnore]
    public bool IsImageType => Type?.StartsWith("image/") == true;

    [JsonIgnore]
    public bool IsAudioType => Type?.StartsWith("audio/") == true;

    [JsonIgnore]
    public bool IsDicomType => Type?.StartsWith("application/dicom") == true;
}
