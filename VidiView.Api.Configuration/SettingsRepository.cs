using System.Diagnostics;
using VidiView.Api.Helpers;
using System.Text;
using VidiView.Api.DataModel;
using SettingCollection = VidiView.Api.Configuration.DataModel.SettingCollection;
using VidiView.Api.Configuration.DataModel;
using System.Net.Http;

namespace VidiView.Api.Configuration;
public class SettingsRepository
{
    readonly HttpClient _http;
    readonly ApiHome _api;
    SettingCollection _settings;

    public SettingsRepository(HttpClient http, Api.DataModel.ApiHome api)
    {
        _http = http;
        _api = api;
    }

    /// <summary>
    /// Set integer value
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public Task<bool> SetDefaultAsync(string key, int value)
    {
        return SetDefaultAsync(key, value.ToString());
    }

    /// <summary>
    /// Set boolean value
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public Task<bool> SetDefaultAsync(string key, bool value)
    {
        return SetDefaultAsync(key, value ? "1" : "0");
    }

    /// <summary>
    /// Set array value
    /// </summary>
    /// <param name="key"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public Task<bool> SetDefaultAsync(string key, string[] values)
    {
        ArgumentNullException.ThrowIfNull(values, nameof(values));

        var sb = new StringBuilder();
        foreach (var v in values)
        {
            sb.Append('"');
            sb.Append(v == null ? "" : v.Replace("\"", "\"\"")) ;
            sb.Append('"');
            sb.Append(',');
        }

        var value = sb.Length == 0 ? "" : sb.ToString(0, sb.Length - 1);
        return SetDefaultAsync(key, value);
    }

    /// <summary>
    /// Set string value
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
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
            var response = await _http.PutAsync(link, updated);
            await response.AssertSuccessAsync();

            settings[key] = updated;
            return true;
        }

        return false;
    }

    public async Task SetOverrideAsync(string key, SettingValueOverride value)
    {
        // Read settings if not already done
        var settings = await GetSettingsAsync();

        var link = settings.Links.GetRequired(Rel.UpdateOverride).AsTemplatedLink();
        link.Parameters["key"].Value = key;

        var response = await _http.PutAsync(link, value);
        await response.AssertSuccessAsync();
    }

    public Task DeleteOverrideAsync(string key, SettingValueOverride value)
    {
        return DeleteOverrideAsync(key, value.OverriddenBy, value.Id);
    }

    public async Task DeleteOverrideAsync(string key, SettingFlags overriddenBy, string id)
    {
        // Read settings if not already done
        var settings = await GetSettingsAsync();

        var link = settings.Links.GetRequired(Rel.DeleteOverride).AsTemplatedLink();
        link.Parameters["key"].Value = key;
        link.Parameters["type"].Value = overriddenBy.ToString();
        link.Parameters["id"].Value = id;

        var response = await _http.DeleteAsync(link);
        await response.AssertSuccessAsync();
    }

    public async Task<SettingCollection> GetSettingsAsync(bool forceReload = false)
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
