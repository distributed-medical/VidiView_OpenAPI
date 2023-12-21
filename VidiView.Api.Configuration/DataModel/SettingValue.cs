namespace VidiView.Configuration.Api;

/// <summary>
/// This class represents a VidiView setting
/// </summary>
public class SettingValue
{
    SettingValueType _valueType;

    public SettingValue()
    {
    }

    public SettingValue(string key, string value)
    {
        Key = key;
        Value = value;
    }

    /// <summary>
    /// Setting key
    /// </summary>
    public string Key { get; init; }

    /// <summary>
    /// Raw value of this setting
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Any configured overrides
    /// </summary>
    public SettingValueOverride[] Overrides { get; set; }

    /// <summary>
    /// General flags
    /// </summary>
    /// <remarks>Only used for exposing Flags over the API</remarks>
    [JsonPropertyName("flags")]
    public SettingFlags? Options { get; init; }

    /// <summary>
    /// Override flags
    /// </summary>
    /// <remarks>Only used for exposing Flags over the API</remarks>
    public SettingFlags? OverrideableBy { get; init; } 

    /// <summary>
    /// This is an id indicating the encryption key used
    /// </summary>
    public uint? EncryptionKeyId { get; init; }

    /// <summary>
    /// Description of this setting
    /// </summary>
    public string? Description { get; set; }

    public override string ToString()
    {
        return $"{Key} = {Value}";
    }
}

public class SettingValueOverride
{
    /// <summary>
    /// The order of override. A higher value is considered more important
    /// </summary>
    public int Order { get; set; }

    public SettingFlags OverriddenBy { get; set; }

    /// <summary>
    /// The entity id that should be matched to be considered for override
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The value of this setting in the overloaded situation
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Optional comment for this setting override
    /// </summary>
    public string Comment { get; set; }

    public override int GetHashCode()
    {
        return $"{OverriddenBy}{Id}{Value}{Comment}".GetHashCode();
    }

    public override string ToString()
    {
        return $"{OverriddenBy} {Id} => {Value}";
    }

    public override bool Equals(object obj)
    {
        if (obj is SettingValueOverride ov)
        {
            return ov.OverriddenBy == OverriddenBy
                && ov.Id == Id
                && ov.Order == Order
                && ov.Comment == Comment;
        }

        return false;
    }
}

