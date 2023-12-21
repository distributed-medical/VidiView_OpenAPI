using VidiView.Api.DataModel;

namespace VidiView.Configuration.Api;

public class SettingCollection
{
    Dictionary<string, SettingValue> _index = new Dictionary<string, SettingValue>();
    SettingValue[] _settings;

    /// <summary>
    /// Number of items in this collection
    /// </summary>
    public int Count
    {
        get; init;
    }

    /// <summary>
    /// The items
    /// </summary>
    public SettingValue[] Items => _settings;

    public SettingValue this[string key] => _index[key];

    /// <summary>
    /// Any HAL Rest links associated with this collection
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection Links
    {
        get; init;
    }

    [JsonPropertyName("_embedded")]
    public EmbeddedArray Embedded
    {
        get => new EmbeddedArray { Settings = _settings };
        set
        {
            if (_settings != null)
                throw new ArgumentException("Embedded property is read-only");
            if (value?.Settings != null)
            {
                _settings = value.Settings;
                var d = new Dictionary<string, SettingValue>();
                foreach (var s in _settings)
                    d.Add(s.Key, s);
                _index = d;
            }
        }
    }

    public class EmbeddedArray
    {
        public SettingValue[] Settings
        {
            get; init;
        }
    }
}
