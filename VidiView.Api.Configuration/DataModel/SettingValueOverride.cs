namespace VidiView.Api.Configuration.DataModel;

public record SettingValueOverride
{
    /// <summary>
    /// The order of override. A higher value is considered more important
    /// </summary>
    public int Order { get; init; }

    /// <summary>
    /// The entity override type
    /// </summary>
    public SettingFlags OverriddenBy { get; init; }

    /// <summary>
    /// The id of the object this override applies to (of type OverriddenBy)
    /// </summary>
    public string Id { get; init; } = null!;

    /// <summary>
    /// The value of this setting in the overloaded situation
    /// </summary>
    public string? Value { get; init; }

    /// <summary>
    /// Optional comment for this setting override
    /// </summary>
    public string? Comment { get; init; }
}
