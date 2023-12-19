using System.Diagnostics;
using VidiView.Api.Access;
using VidiView.Api.DataModel;

namespace VidiView.Api.Configuration;
public class SettingsRepository
{
    readonly HttpClient _http;
    readonly ApiHome _api;
    SettingCollection _settings;

    internal SettingsRepository(HttpClient http, ApiHome api)
    {
        _http = http;
        _api = api;
    }

    public Task<bool> SetDefaultAsync(string key, int value)
    {
        return SetDefaultAsync(key, value.ToString());
    }

    public Task<bool> SetDefaultAsync(string key, bool value)
    {
        return SetDefaultAsync(key, value ? "1" : "0");
    }

    public async Task<bool> SetDefaultAsync(string key, string value)
    {
        // Read settings if not already done
        var settings = await GetSettingsAsync();
        var setting = settings[key] ?? throw new KeyNotFoundException($"The setting key {key} does not exist");

        if (setting.Value != value)
        {
            Debug.WriteLine($"Updating setting {key} value to {value}");
            var link = settings.Links.GetRequired(Rel.Update);

            var updated = setting with { Value = value };
            await _http.PutAsync(link.ToUrl(), HttpContentFactory.CreateBody(updated));

            settings[key] = updated;
            return true;
        }

        return false;
    }

    async Task<SettingCollection> GetSettingsAsync(bool forceReload = false)
    {
        var settings = _settings;
        if (settings == null || forceReload)
        {
            settings = await _http.GetAsync<SettingCollection>(_api.Links.GetRequired(Rel.Settings));
            _settings = settings;
        }

        return settings;
    }
}
