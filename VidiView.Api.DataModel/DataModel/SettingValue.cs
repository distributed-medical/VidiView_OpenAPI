namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record SettingValue
{
    /// <summary>
    /// The setting key
    /// </summary>
    public string Key { get; init; } = null!;

    /// <summary>
    /// The setting value, or null if not set
    /// </summary>
    /// <remarks>
    /// This value is normally the value applicable to the user, role and department. 
    /// In configuration mode, this is the default value for the setting, and any overrides are in the Overrides property.
    /// </remarks>
    public string? Value { get; init; }

    /// <summary>
    /// Description of this setting. Only populated in configuration mode.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Configured overrides. Only populated in configuration mode.
    /// </summary>
    public SettingValueOverride[]? Overrides { get; init; }

    /// <summary>
    /// General flags
    /// </summary>
    public SettingFlags? Flags { get; init; }

    public override string ToString()
    {
        return $"{Key} = {Value}";
    }
}
