using System.Net.Http;
using VidiView.Api.Authentication;
using VidiView.Api.DataModel;
using VidiView.Api.Helpers;

namespace Integration.Test;

/// <summary>
/// This class demonstrates uploading an image to a known
/// study on the VidiView Server
/// </summary>
[TestClass]
public class UploadImage
{
    static HttpClient _http = null!;

    [ClassInitialize]
    public static async Task ClassInitialize(TestContext context)
    {
        // Prepare HttpClient with API key
        var apiKey = TestConfig.ApiKey();
        _http = new HttpClient();
        _http.DefaultRequestHeaders.Add(apiKey.Name, apiKey.Value);

        // Connect to VidiView and register device (or update device registration)
        await _http.ConnectAsync(TestConfig.ServerHostName, CancellationToken.None);
        var device = await _http.RegisterDeviceAsync("4.0", "Apple iPhone 15");
        if (!device.IsGranted)
            throw new Exception($"Device not granted access, registration token: {device.RegistrationToken}");

        // Authenticate
        var auth = new UsernamePasswordAuthenticator(_http);
        await auth.AuthenticateAsync(
            TestConfig.Username,
            TestConfig.Password);

        // Now we are good to go!
    }

    /// <summary>
    /// Load a study from a known id
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task LoadSpecificStudy()
    {
        var api = await _http.HomeAsync();

        // Load a specific study
        var link = api.Links.GetRequired(Rel.Study).AsTemplatedLink();
        link.Parameters["studyId"].Value = TestConfig.StudyId;

        var study = await _http.GetAsync<Study>(link);
        Assert.AreEqual(new Guid("b71f7d6e-aadf-4d38-9343-f3e88250f6e5"), study.StudyId);
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
            Checksum = CalculateChecksum(fileStream)
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