using VidiView.Api.Helpers;
using VidiView.Api.DataModel;
using System.Net.Http;
using System.Diagnostics;

namespace VidiView.Api.Configuration;

/// <summary>
/// This class provides methods to manage devices registered with the VidiView system. It allows setting the 
/// granted state of a device and deleting device registrations.
/// </summary>
public class EquipmentManager
{
    readonly HttpClient _http;
    readonly ApiHome _api;
    LinkCollection? _links;

    internal EquipmentManager(HttpClient http, ApiHome api)
    {
        _http = http;
        _api = api;
    }

    /// <summary>
    /// List all registered equipment
    /// </summary>
    public async Task<IReadOnlyList<Equipment>> ListAsync(Guid? departmentId = null)
    {
        var link = _api.Links.GetRequired(Rel.Equipment).AsTemplatedLink();
        link.TrySetParameterValue("departmentId", departmentId?.ToString() ?? "any"); 
        
        var result = await _http.GetAsync<EquipmentCollection>(link).ConfigureAwait(false);
        _links = result.Links;
        return result.Items;
    }

    /// <summary>
    /// Load specific equipment
    /// </summary>
    public async Task<Equipment> LoadAsync(Guid equipmentId)
    {
        var link = _api.Links.GetRequired(Rel.Equipment).AsTemplatedLink();
        link.TrySetParameterValue("departmentId", "any");
        link.Parameters["id"].Value = equipmentId.ToString();

        var result = await _http.GetAsync<Equipment>(link).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Save equipment. 
    /// </summary>
    /// <remarks>Set equipment.Id to Guid.Empty to create new equipment. 
    /// Otherwise this will create or update equipment, which requires more privileges</remarks>
    public async Task<Equipment> SaveAsync(Equipment equipment)
    {
        if (_links == null)
        {
            _ = await ListAsync().ConfigureAwait(false);
            Debug.Assert(_links != null);
        }

        var link = _links.GetRequired(Rel.Update).AsTemplatedLink();
        link.TrySetParameterValue("departmentId", equipment.Department.Id.ToString());

        var response = await _http.PutAsync(link, equipment).ConfigureAwait(false);
        await response.AssertSuccessAsync().ConfigureAwait(false);

        var result = await response.DeserializeAsync<Equipment>().ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Delete equipment. 
    /// </summary>
    /// <remarks>Set equipment.Id to Guid.Empty to create new equipment. 
    /// Otherwise this will create or update equipment, which requires more privileges</remarks>
    public async Task DeleteAsync(Guid equipmentId)
    {
        if (_links == null)
        {
            _ = await ListAsync().ConfigureAwait(false);
            Debug.Assert(_links != null);
        }

        var link = _links.GetRequired(Rel.Delete).AsTemplatedLink();
        link.Parameters["id"].Value = equipmentId.ToString();

        var response = await _http.DeleteAsync(link).ConfigureAwait(false);
        await response.AssertSuccessAsync().ConfigureAwait(false);
    }
}
