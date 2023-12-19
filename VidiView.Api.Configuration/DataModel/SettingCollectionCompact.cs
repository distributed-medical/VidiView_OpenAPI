using System.Collections;

namespace VidiView.Configuration.Api;

public class SettingCollectionCompact : IReadOnlyDictionary<string, string>
{
    readonly Dictionary<string, string> _settings;

    public SettingCollectionCompact()
    {
        _settings = new Dictionary<string, string>();
    }

    public SettingCollectionCompact(SettingValue[] settings)
    {
        _settings = new Dictionary<string, string>();
        if (settings != null)
        {
            foreach (var s in settings)
            {
                _settings.Add(s.Key, s.Value);
            }
        }
    }

    public string this[string key] => ((IReadOnlyDictionary<string, string>)_settings)[key];

    public IEnumerable<string> Keys => ((IReadOnlyDictionary<string, string>)_settings).Keys;

    public IEnumerable<string> Values => ((IReadOnlyDictionary<string, string>)_settings).Values;

    public int Count => ((IReadOnlyCollection<KeyValuePair<string, string>>)_settings).Count;

    public bool ContainsKey(string key)
    {
        return ((IReadOnlyDictionary<string, string>)_settings).ContainsKey(key);
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return ((IEnumerable<KeyValuePair<string, string>>)_settings).GetEnumerator();
    }

    public bool TryGetValue(string key, out string value)
    {
        return ((IReadOnlyDictionary<string, string>)_settings).TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_settings).GetEnumerator();
    }
}
