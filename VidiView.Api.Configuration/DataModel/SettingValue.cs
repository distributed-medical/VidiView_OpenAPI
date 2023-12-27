namespace VidiView.Api.Configuration.DataModel;

public record SettingValue : VidiView.Api.DataModel.SettingValue
{
    /// <summary>
    /// Any configured overrides
    /// </summary>
    public SettingValueOverride[]? Overrides { get; init; }

    /// <summary>
    /// Description of this setting
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// General flags
    /// </summary>
    public SettingFlags? Flags { get; init; }
    
    /// <summary>
    /// Identifies the encryption key to use for decrypting this setting on the server
    /// </summary>
    public uint? EncryptionKeyId { get; init; }

    public override string ToString()
    {
        return $"{Key} = {Value}";
    }
}
