namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record SettingValue
{
    public string Key { get; init; } = null!;
    public string? Value { get; init; }
}
