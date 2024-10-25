using System.Net.Http.Headers;
using System.Text;
using VidiView.Api.DataModel;
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

        // var studyUri = (Uri)study.Links.GetRequired(Rel.Self).AsTemplatedLink();
        // await CallExternalApplication(studyUri, restrictedToken.Token);

        var uploadUrl = study.Links.GetRequired(Rel.UploadMedia).Href; // Parameters are not parsed
        await ZeroIntegrationContributeImage(uploadUrl, restrictedToken.Token);
    }

    /// <summary>
    /// Simulate call to external system that can contribute to a specific study
    /// </summary>
    /// <param name="studyUri"></param>
    /// <param name="deviceThumbprint"></param>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    private async Task CallExternalApplication(Uri studyUri, string accessToken)
    {
        // Create a new, independent HttpClient
        var handler = new DebugLogHandler(HttpClientHandlerFactory.Create());
        var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(10) };

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

    /// <summary>
    /// This example showcases upload of image and metadata, without
    /// any use of our packages
    /// </summary>
    /// <param name="url">The url to upload to</param>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    private async Task ZeroIntegrationContributeImage(string url, string accessToken)
    {
        // Required VidiView image id, in Guid format
        Guid imageId = Guid.NewGuid();
        // Optional image id, in OID format (this is kept even when exporting to long term storage)
        string sopInstanceUid = $"1.2.3.4.{DateTime.UtcNow.Ticks}";

        string metadata = @"{ ""original-filename"": ""photo.jpg"", 
            ""name"": ""Cool image"", 
            ""sop-instance-uid"": ""{sopInstanceUid}""
        }";

        url = url.Replace("{imageId}", imageId.ToString());
        metadata = metadata.Replace("{sopInstanceUid}", sopInstanceUid);

        // File to upload
        using var fileStream = new FileStream("TestData\\VietBun.jpg", FileMode.Open, FileAccess.Read, FileShare.Read);

        var part1 = new StringContent(metadata, Encoding.UTF8, new MediaTypeHeaderValue("application/json"));
        var part2 = new StreamContent(fileStream);
        part2.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

        var multiPartContent = new MultipartFormDataContent();
        multiPartContent.Add(part1);
        multiPartContent.Add(part2);

        // Create a new, independent HttpClient
        var http = new HttpClient();

        // Prepare POST request
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Content = multiPartContent;

        // Send request
        var response = await http.SendAsync(request);

        // Verify success
        if ((int)response.StatusCode < 300)
        {
            // Success
            Assert.AreEqual("application/hal+json", response.Content.Headers.ContentType?.MediaType);
            var mediaFile = await response.Content.ReadAsStringAsync();
        }
        else
        {
            Assert.AreEqual("application/problem+json", response.Content.Headers.ContentType?.MediaType);
            var problemDetails = await response.Content.ReadAsStringAsync();

            Assert.IsTrue(false, problemDetails);
        }
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
            Lifetime = TimeSpan.FromHours(24),
            AppId = TestConfig.ExternalAppId,
            ContributeStudy = [studyId] // Only allow contribution to the specific study
        };

        var response = await _http.PostAsync(exchangeTokenLink, restricted, CancellationToken.None);
        await response.AssertSuccessAsync();
        var token = response.Deserialize<AuthToken>();

        // Allow for some time skew between client and server
        Assert.IsTrue(token.Expires > DateTime.UtcNow.AddHours(24).AddMinutes(-5));
        Assert.IsTrue(token.Expires < DateTime.UtcNow.AddHours(24).AddMinutes(5));
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
        Assert.IsTrue(patients.Count > 0, "No patient found in system with the specified ID");

        // Link to create a study in specific department
        var tl = patients.Items[0].Links.GetRequired(Rel.CreateStudy).AsTemplatedLink();
        tl.TrySetParameterValue("departmentId", TestConfig.DepartmentId.ToString());

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