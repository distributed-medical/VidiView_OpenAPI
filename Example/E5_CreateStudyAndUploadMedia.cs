using VidiView.Api.DataModel;
using VidiView.Api.Helpers;
using VidiView.Example.TestData;

namespace VidiView.Example;

/// <summary>
/// This class demonstrates uploading an image to a known
/// study on the VidiView Server
/// </summary>
[TestClass]
public class E5_CreateStudyAndUploadMedia
{
    static HttpClient _http = null!;

    [ClassInitialize]
    public static async Task ClassInitialize(TestContext context)
    {
        _http = await HttpClientFactory.CreateAsync();

    }

    /// <summary>
    /// Create an unidentified study in the system
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task Create_Unidentified_Study()
    {
        var api = await _http.HomeAsync();

        // Get the first department allowing us to create a study
        var departments = await _http.GetAsync<DepartmentCollection>(api.Links.GetRequired(Rel.Departments));
        var department = departments.Items.FirstOrDefault((d) => d.Links.Exists(Rel.CreateStudy));
        Assert.IsNotNull(department, "You are not granted Create study rights in any department");

        var link = department.Links.GetRequired(Rel.CreateStudy);
        string s = DateTime.UtcNow.Ticks.ToString();
        if (s.Length > 15)
            s = s.Substring(s.Length - 15);

        // Initial parameters to assign to this study
        var sco = new StudyCreateOptions
        {
            AccessionNumber = $"T{s}",
            Patient = null, 
            AssignToSelf = false
        };

        var response = await _http.PostAsync(link, sco);
        await response.AssertSuccessAsync();
        var study = response.Deserialize<Study>();

        // Now we have a study
        Assert.AreEqual(sco.AccessionNumber, study.AccessionNumber);

        // Since we didn't assign the study in the first call...
        Assert.IsTrue(study.Links.Exists(Rel.Assign));
    }

    /// <summary>
    /// Upload an image to a specific study
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task UploadImageToStudy()
    {
        var api = await _http.HomeAsync();

        // Load a specific study
        var tl = api.Links.GetRequired(Rel.Study).AsTemplatedLink();
        tl.Parameters["studyId"].Value = TestConfig.StudyId;
        var study = await _http.GetAsync<Study>(tl);

        // File to upload
        using var fileStream = new FileStream("TestData\\VietBun.jpg", FileMode.Open, FileAccess.Read, FileShare.Read);

        // Start by announcing the upload, including file metadata
        var announceLink = study.Links.GetRequired(Rel.AnnounceUpload);
        var mediaFile = new MediaFile
        {
            ImageId = Guid.NewGuid(),
            ContentType = "image/jpeg",
            FileSize = fileStream.Length,
            OriginalFilename = "VietBun.jpg",
            Checksum = CalculateChecksum(fileStream),
            Name = "Nice lunch",
            Description = "Vietnamese Bao-Bun",
        };

        // Post data to the server
        var response = await _http.PostAsync(announceLink, mediaFile);
        await response.AssertSuccessAsync();

        // Deserialize the response
        var createdImage = response.Deserialize<MediaFile>();

        // Where should we upload the file?
        var uploadLink = createdImage.Links.GetRequired(Rel.UploadMedia);

        // Upload the content
        var uploadHelper = new UploadHelper(_http);
        var finalImage = await uploadHelper.UploadWithResumeAsync(uploadLink,
            fileStream,
            mediaFile.ContentType,
            CancellationToken.None);

        Assert.AreEqual(fileStream.Length, finalImage.FileSize);
    }

    static string CalculateChecksum(Stream stream)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var hash = sha256.ComputeHash(stream);
        stream.Position = 0;

        var sb = new System.Text.StringBuilder(hash.Length * 2);
        foreach (var b in hash)
            sb.AppendFormat("{0:x2}", b);
        return sb.ToString();
    }

}