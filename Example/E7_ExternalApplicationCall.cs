using System.Net.Http.Headers;
using VidiView.Api.Authentication;
using VidiView.Api.DataModel;
using VidiView.Api.Headers;
using VidiView.Api.Helpers;
using VidiView.Example.HttpHandlers;
using VidiView.Example.TestData;

namespace VidiView.Example;

/// <summary>
/// This class demonstrates creating a restricted access token
/// to grant an external application access to VidiView
/// </summary>
[TestClass]
public class E7_ExternalApplicationCall
{
    static HttpClient _http = null!;

    [ClassInitialize]
    public static async Task ClassInitialize(TestContext context)
    {
        _http = await HttpClientFactory.CreateAsync();
    }

    /// <summary>
    /// Create a study for patient 
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task Prepare_Study_And_Get_Contribution_Token()
    {
        var study = await CreateStudyForPatientAsync();
        var restrictedToken = await CreateContributionToken(study.StudyId);
        var studyUri = (Uri)study.Links.GetRequired(Rel.Self).AsTemplatedLink();

        await CallExternalApplication(studyUri, TestConfig.Thumbprint, restrictedToken.Token);
    }

    /// <summary>
    /// Simulate call to external system that can contribute to a specific study
    /// </summary>
    /// <param name="studyUri"></param>
    /// <param name="deviceThumbprint"></param>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    private async Task CallExternalApplication(Uri studyUri, byte[] deviceThumbprint, string accessToken)
    {
        // Create a new, independent HttpClient
        var handler = new DebugLogHandler(HttpClientHandlerFactory.Create());
        var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(10) };

        // Api-key for the external application
        var appId = TestConfig.ExternalAppId;
        var apiKey = TestConfig.ExternalAppKey;
        http.SetApiKey(new ApiKeyHeader(appId, deviceThumbprint, apiKey));

        // Use the received access token as-is
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        // Get study metadata
        var response = await http.GetAsync(studyUri);
        await response.AssertSuccessAsync();
        var study = response.Deserialize<Study>();

        // We should be able to contribute/upload at this point,
        // but not edit the study metadata itself
        Assert.IsTrue(study.Links.Exists(Rel.AnnounceUpload));
        Assert.IsTrue(study.Links.Exists(Rel.UploadMedia));
        Assert.IsFalse(study.Links.Exists(Rel.Update));

        // To upload media at this point, please look at example E6
    }

    private async Task<AuthToken> CreateContributionToken(Guid studyId)
    {
        var start = await _http.HomeAsync();
        var link = start.Links.GetRequired(Rel.UserProfile);

        var user = await _http.GetAsync<User>(link);
        var exchangeTokenLink = user.Links.GetRequired(Rel.RequestToken);

        // Get a token with limited access
        var restricted = new TokenRequest
        {
            Lifetime = TimeSpan.FromDays(30),
            AppId = TestConfig.ExternalAppId,
            ContributeStudy = [studyId] // Only allow contribution to the specific study
        };

        var response = await _http.PostAsync(exchangeTokenLink, restricted, CancellationToken.None);
        await response.AssertSuccessAsync();
        var token = response.Deserialize<AuthToken>();

        Assert.IsTrue(token.Expires > DateTime.UtcNow.AddDays(30).AddMinutes(-1));
        return token;
    }

    private static async Task<Study> CreateStudyForPatientAsync()
    {
        var start = await _http.HomeAsync();
        var link = start.Links.GetRequired(Rel.FindPatient);
        var criteria = new PatientSearch { IdNumber = TestConfig.PatientId };

        var result = await _http.PostAsync(link, criteria);
        await result.AssertSuccessAsync();
        var patients = result.Deserialize<PatientCollection>();

        // Link to create a study in specific department
        var tl = patients.Items[0].Links.GetRequired(Rel.CreateStudy).AsTemplatedLink();
        tl.TrySetParameterValue("departmentId", TestConfig.DepartmentId.ToString() );

        // Initial parameters to assign to this study
        var sco = new StudyCreateOptions
        {
            AssignToSelf = false
        };

        var response = await _http.PostAsync(tl, sco, CancellationToken.None);
        await response.AssertSuccessAsync();
        var study = response.Deserialize<Study>();

        return study;
    }
}