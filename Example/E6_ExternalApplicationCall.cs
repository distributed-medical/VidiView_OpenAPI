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
/// to grant an external application access to VidiVIew
/// </summary>
[TestClass]
public class E6_ExternalApplicationCall
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
    public async Task Exchange_Token()
    {
        var study = await CreateStudyForPatientAsync();
        var restrictedToken = await CreateRestrictedToken(study.StudyId);
        var studyUri = (Uri)study.Links.GetRequired(Rel.Self).AsTemplatedLink();

        await ExternalCallAsync(studyUri, TestConfig.Thumbprint, restrictedToken.Token);
    }

    async Task ExternalCallAsync(Uri studyUri, byte[] deviceThumbprint, string accessToken)
    {
        // This simulates the called application which should now
        // be able to contribute with files to the study

        var handler = new DebugLogHandler(HttpClientHandlerFactory.Create());
        var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(10) };
        http.SetApiKey(new ApiKeyHeader(TestConfig.ApplicationId, deviceThumbprint, TestConfig.SecretKey));
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
    }

    public async Task<AuthToken> CreateRestrictedToken(Guid studyId)
    {
        var start = await _http.HomeAsync();
        var link = start.Links.GetRequired(Rel.UserProfile);

        var user = await _http.GetAsync<User>(link);
        var exchangeTokenLink = user.Links.GetRequired(Rel.RequestToken);

        // Get a token with limited access
        var restricted = new TokenRequest
        {
            Lifetime = TimeSpan.FromDays(30),
            Scope = "vidiview-patient:read",
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
        var criteria = new PatientSearch { IdNumber = "197603221239" };

        var result = await _http.PostAsync(link, criteria);
        await result.AssertSuccessAsync();
        var patients = result.Deserialize<PatientCollection>();

        // Link to create a study in specific department
        var tl = patients.Items[0].Links.GetRequired(Rel.CreateStudy).AsTemplatedLink();
        tl.TrySetParameterValue("departmentId", "3b8d2e01-3f9e-4061-9ca5-8eb435d2b071");

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