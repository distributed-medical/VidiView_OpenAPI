namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record SettingCollection
{
    /// <summary>
    /// Number of items in this collection
    /// </summary>
    public int Count { get; init; }

    /// <summary>
    /// Any HAL Rest links associated with this collection
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }

    [JsonPropertyName("_embedded")]
    public EmbeddedArray Embedded { get; init; } = null!;

    public SettingValue this[string key]
    {
        get => Embedded.Settings.FirstOrDefault((i) => i.Key == key)
            ?? throw new ArgumentException($"The setting {key} does not exist.");
    }

    public class EmbeddedArray
    {
        public SettingValue[] Settings { get; init; }
    }

    /// <summary>
    /// Used by Vidiview Capture
    /// </summary>
    public Dictionary<string, string>? Settings { get; init; }

}
