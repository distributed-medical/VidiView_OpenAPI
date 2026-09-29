using System.Net.Http;
using VidiView.Api.DataModel;
using VidiView.Api.Helpers;

namespace VidiView.Api.Configuration;

public class AuditLogQuery
{
    readonly HttpClient _http;
    readonly ApiHome _api;

    public AuditLogQuery(HttpClient http, ApiHome api)
    {
        _http = http;
        _api = api;
    }

    /// <summary>
    /// Return category and name for event id's that are logged in the audit log. 
    /// </summary>
    /// <returns></returns>
    public async Task<IReadOnlyList<AuditEventId>> GetEventNames()
    {
        var link = _api.Links.GetRequired(Rel.AuditEvents).AsTemplatedLink();
        var result = await _http.GetAsync<AuditEventId[]>(link).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Return all events for a specific user within a given time period. The events are returned in descending order of event time.
    /// </summary>
    /// <param name="userId">The user to query</param>
    /// <param name="fromDate">The start date of the range</param>
    /// <param name="toDate">The end date of the range</param>
    /// <param name="filterByEventId">Optional set of event IDs to include. Pass null to include all events.</param>
    /// <param name="maxHits"></param>
    /// <returns>A single log event for each patient the user has had interactions with</returns>
    /// <remarks>The event time is the time of the latest logged event in the supplied range</remarks>
    public async Task<AuditEventCollection> GetLogForUser(Guid userId, DateTime fromDate, DateTime toDate, IEnumerable<int>? filterByEventId = null, int maxHits = 5000)
    {
        var link = _api.Links.GetRequired(Rel.AuditLogForUser).AsTemplatedLink();
        link.TrySetParameterValue("userId", userId.ToString("N"));
        link.TrySetParameterValue("fromDate", fromDate.ToString("yyyyMMdd"));
        link.TrySetParameterValue("toDate", toDate.ToString("yyyyMMdd"));
        link.TrySetParameterValue("maxHits", maxHits.ToString());

        if (filterByEventId != null)
        {
            var ids = filterByEventId.ToArray();
            if (ids.Length > 0)
                link.TrySetParameterValue("event", string.Join(",", ids));
        }

        var result = await _http.GetAsync<AuditEventCollection>(link).ConfigureAwait(false);
        return result;
    }


    /// <summary>
    /// Return all events for a specific study within a given time period. The events are returned in descending order of event time.
    /// </summary>
    /// <param name="studyId"></param>
    /// <param name="fromDate"></param>
    /// <param name="toDate"></param>
    /// <param name="maxHits"></param>
    /// <returns></returns>
    public async Task<AuditEventCollection> GetLogForStudy(Guid studyId, DateTime fromDate, DateTime toDate, int maxHits = 5000)
    {
        var link = _api.Links.GetRequired(Rel.AuditLogForStudy).AsTemplatedLink();
        link.TrySetParameterValue("studyId", studyId.ToString("N"));
        link.TrySetParameterValue("fromDate", fromDate.ToString("yyyyMMdd"));
        link.TrySetParameterValue("toDate", toDate.ToString("yyyyMMdd"));
        link.TrySetParameterValue("maxHits", maxHits.ToString());

        var result = await _http.GetAsync<AuditEventCollection>(link).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Return a list of distinct patient records a specific user has had interactions within a given time period.
    /// </summary>
    /// <param name="userId">The user to query</param>
    /// <param name="fromDate">The start date of the range</param>
    /// <param name="toDate">The end date of the range</param>
    /// <returns>A single log event for each patient the user has had interactions with</returns>
    /// <remarks>The event time is the time of the latest logged event in the supplied range</remarks>
    public async Task<AuditEventCollection> GetDistinctPatientInteractions(Guid userId, DateTime fromDate, DateTime toDate, int maxHits = 1000)
    {
        var link = _api.Links.GetRequired(Rel.AuditLogPatientInteractions).AsTemplatedLink();
        link.TrySetParameterValue("userId", userId.ToString("N"));
        link.TrySetParameterValue("fromDate", fromDate.ToString("yyyyMMdd"));
        link.TrySetParameterValue("toDate", toDate.ToString("yyyyMMdd"));
        link.TrySetParameterValue("includeDepartment", "0");
        link.TrySetParameterValue("maxHits", maxHits.ToString());

        var result = await _http.GetAsync<AuditEventCollection>(link).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Return a list of patient records a specific user has had interactions within a given time period.
    /// </summary>
    /// <param name="userId">The user to query</param>
    /// <param name="fromDate">The start date of the range</param>
    /// <param name="toDate">The end date of the range</param>
    /// <returns>A log event for each day, department and patient the user has had interactions with</returns>
    /// <remarks>The event time is the time of the first logged event during the day</remarks>
    public async Task<AuditEventCollection> GetPatientInteractionsPerDepartmentAndDay(Guid userId, DateTime fromDate, DateTime toDate, int maxHits = 1000)
    {
        var link = _api.Links.GetRequired(Rel.AuditLogPatientInteractions).AsTemplatedLink();
        link.TrySetParameterValue("userId", userId.ToString("N"));
        link.TrySetParameterValue("fromDate", fromDate.ToString("yyyyMMdd"));
        link.TrySetParameterValue("toDate", toDate.ToString("yyyyMMdd"));
        link.TrySetParameterValue("includeDepartment", "1");
        link.TrySetParameterValue("maxHits", maxHits.ToString());

        var result = await _http.GetAsync<AuditEventCollection>(link).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Return a list of patient records a specific user has had interactions within a given time period.
    /// </summary>
    /// <param name="userId">The user to query</param>
    /// <param name="fromDate">The start date of the range</param>
    /// <param name="toDate">The end date of the range</param>
    /// <returns>A log event for each day, department and patient the user has had interactions with</returns>
    /// <remarks>The event time is the time of the first logged event during the day</remarks>
    public async Task<AuditEventCollection> GetStudyInteractions(Guid userId, Guid patientIdGuid, DateTime fromDate, DateTime toDate, int maxHits = 5000)
    {
        var link = _api.Links.GetRequired(Rel.AuditLogStudyInteractions).AsTemplatedLink();
        link.TrySetParameterValue("userId", userId.ToString("N"));
        link.TrySetParameterValue("patientIdGuid", patientIdGuid.ToString("N"));
        link.TrySetParameterValue("fromDate", fromDate.ToString("yyyyMMdd"));
        link.TrySetParameterValue("toDate", toDate.ToString("yyyyMMdd"));
        link.TrySetParameterValue("maxHits", maxHits.ToString());

        var result = await _http.GetAsync<AuditEventCollection>(link).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Return a list of distinct users that have had interactions with a specific patient within a given time period.
    /// </summary>
    /// <param name="patientIdGuid"></param>
    /// <param name="fromDate"></param>
    /// <param name="toDate"></param>
    /// <param name="maxHits"></param>
    /// <returns></returns>
    public async Task<AuditEventCollection> GetDistinctUsersForPatient(Guid patientIdGuid, DateTime fromDate, DateTime toDate, int maxHits = 1000)
    {
        var link = _api.Links.GetRequired(Rel.AuditLogForPatient).AsTemplatedLink();
        link.TrySetParameterValue("patientIdGuid", patientIdGuid.ToString("N"));
        link.TrySetParameterValue("fromDate", fromDate.ToString("yyyyMMdd"));
        link.TrySetParameterValue("toDate", toDate.ToString("yyyyMMdd"));
        link.TrySetParameterValue("maxHits", maxHits.ToString());

        var result = await _http.GetAsync<AuditEventCollection>(link).ConfigureAwait(false);
        return result;
    }



}
