namespace VidiView.Configuration.Api;

/// <summary>
/// This class represents a VidiView setting
/// </summary>
public class SettingDefinition
{
    /// <summary>
    /// Setting key
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// The default value of this setting
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// XML element describing the datatype of this setting
    /// </summary>
    public string DataType { get; set; }

    /// <summary>
    /// Options for this setting
    /// </summary>
    public long Flags { get; set; }

    /// <summary>
    /// Optional setting description
    /// </summary>
    public string Description { get; set; }

    public SettingDefinition Clone()
    {
        return (SettingDefinition)MemberwiseClone();
    }
}
