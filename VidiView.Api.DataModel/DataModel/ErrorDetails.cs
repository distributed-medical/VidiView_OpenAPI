namespace VidiView.Api.DataModel;

[Obsolete("Use ProblemDetails instead", true)]
public record ErrorDetails
{
    public string ErrorCode { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string Type { get; init; } = null!;
}
