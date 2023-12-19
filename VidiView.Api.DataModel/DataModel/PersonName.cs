namespace VidiView.Api.DataModel;

public record PersonName 
{
    public string Display { get; init; } = null!;

    [JsonPropertyName("parts")]
    public string? HatFormat { get; init; }

    public override string ToString() => Display ?? HatFormat ?? "<null>";
}
