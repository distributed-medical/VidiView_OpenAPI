namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record VideoExtractFrameRequest
{
    public TimeSpan Position { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
}
