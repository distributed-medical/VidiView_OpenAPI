namespace VidiView.Api.DataModel;

public record SettingValue
{
    public string Key { get; init; } = null!;
    public string? Value { get; init; }
}
