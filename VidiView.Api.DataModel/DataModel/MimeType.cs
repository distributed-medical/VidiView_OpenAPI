namespace VidiView.Api.DataModel;

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
    public bool IsVideoType => Type.StartsWith("video/");

    [JsonIgnore]
    public bool IsImageType => Type.StartsWith("image/");

    [JsonIgnore]
    public bool IsAudioType => Type.StartsWith("audio/");

    [JsonIgnore]
    public bool IsDicomType => Type.StartsWith("application/dicom");
}
