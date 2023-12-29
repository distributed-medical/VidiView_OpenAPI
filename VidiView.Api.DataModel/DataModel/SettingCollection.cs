namespace VidiView.Api.DataModel;

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

        set
        {
            ArgumentNullException.ThrowIfNull(key, nameof(key));
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            for (var i = 0; i < Embedded.Settings.Length; ++i)
            {
                if (Embedded.Settings[i].Key == key)
                {
                    Embedded.Settings[i] = value;
                    return;
                }
            }

            throw new KeyNotFoundException("The specified setting does not exist");
        }
    }

    public class EmbeddedArray
    {
        public SettingValue[] Settings { get; init; }
    }
}
