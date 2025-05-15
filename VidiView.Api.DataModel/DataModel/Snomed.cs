namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record Snomed
{
    public string Expression { get; init; }

    public string? Compact { get; init; }

    public string? Meaning { get; init; }
}