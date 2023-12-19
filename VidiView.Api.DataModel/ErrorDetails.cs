namespace VidiView.Api.DataModel;

public record ErrorDetails
{
    public string ErrorCode { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string Type { get; init; } = null!;
}
