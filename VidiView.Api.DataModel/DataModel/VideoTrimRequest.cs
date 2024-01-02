namespace VidiView.Api.DataModel;

public record VideoTrimRequest
{
    public TimeSpan StartPosition { get; init; }
    public TimeSpan EndPosition { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
}
