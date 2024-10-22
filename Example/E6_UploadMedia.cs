using VidiView.Api.DataModel;
using VidiView.Api.Helpers;
using VidiView.Example.TestData;

namespace VidiView.Example;

/// <summary>
/// This class demonstrates uploading an image to a known
/// study on the VidiView Server.
/// There are several ways to upload images, supporting different
/// scenarios and needs.
/// </summary>
[TestClass]
public class E6_UploadMedia
{
    static HttpClient _http = null!;

    [ClassInitialize]
    public static async Task ClassInitialize(TestContext context)
    {
        _http = await HttpClientFactory.CreateAsync();

    }

    /// <summary>
    /// This is the simplest type of upload. Just POST a stream to 
    /// the intended study and the system will handle the rest.
    /// No metadata about the image can be assigned with this approach.
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task Unannounced_Upload()
    {
        var api = await _http.HomeAsync();

        // Load a specific study
        var tl = api.Links.GetRequired(Rel.Study).AsTemplatedLink();
        tl.Parameters["studyId"].Value = TestConfig.StudyId;
        var study = await _http.GetAsync<Study>(tl);

        var link = study.Links.GetRequired(Rel.UploadMedia).AsTemplatedLink();

        // The only thing we need to do is specify the image id (must be of Guid type) in the upload Uri
        Guid imageId = Guid.NewGuid();
        link.TrySetParameterValue("imageId", imageId.ToString());

        // Post file stream to the server
        using var fileStream = new FileStream("TestData\\VietBun.jpg", FileMode.Open, FileAccess.Read, FileShare.Read);
        var httpContent = HttpContentFactory.CreateBody(fileStream, "image/jpeg");
        var response = await _http.PostAsync(link.ToUrl(), httpContent, CancellationToken.None);

        await response.AssertSuccessAsync();

        // Deserialize the response body
        var createdImage = response.Deserialize<MediaFile>();

        Assert.AreEqual(imageId, createdImage.ImageId);
        Assert.AreEqual("image/jpeg", createdImage.ContentType);
        Assert.AreEqual(fileStream.Length, createdImage.FileSize);
        Assert.AreEqual(4032, createdImage.Width);
        Assert.AreEqual(3024, createdImage.Height);
    }

    /// <summary>
    /// By first announcing the image to the system, metadata can be added to the image.
    /// If a file checksum is included with the announcement, the system will verify the
    /// received file's integrity. This also enables upload continuation for interrupted transfers
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task Announced_Upload()
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
            SopInstanceUid = $"1.2.3.4.{DateTime.UtcNow.Ticks}",
            ContentType = "image/jpeg",
            FileSize = fileStream.Length,
            Checksum = CalculateChecksum(fileStream),
            OriginalFilename = "VietBun.jpg",
            Name = "Nice lunch",
            Description = "Vietnamese Bao-Bun",
        };

        // Announce/create image on the server
        var response = await _http.PostAsync(announceLink, mediaFile);
        await response.AssertSuccessAsync();

        // Deserialize the response
        var createdImage = response.Deserialize<MediaFile>();

        // Where should we upload the file?
        var uploadLink = createdImage.Links.GetRequired(Rel.UploadMedia);

        // Upload the content. In this example we use the helper method
        // to support resuming the upload. This helper will internally query
        // the server to see if any interrupted upload can be continued upon.
        var uploadHelper = new UploadHelper(_http);
        var finalImage = await uploadHelper.UploadWithResumeAsync(uploadLink,
            fileStream,
            mediaFile.ContentType,
            CancellationToken.None);

        Assert.AreEqual(fileStream.Length, finalImage.FileSize);
    }

    /// <summary>
    /// Announce and upload can be performed as a single task by submitting
    /// a multipart form data content with the first part containing the metadata,
    /// and the second part containing the actual file content
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task MultiPart_Upload()
    {
        var api = await _http.HomeAsync();

        // Load a specific study
        var tl = api.Links.GetRequired(Rel.Study).AsTemplatedLink();
        tl.Parameters["studyId"].Value = TestConfig.StudyId;
        var study = await _http.GetAsync<Study>(tl);

        // File to upload
        using var fileStream = new FileStream("TestData\\VietBun.jpg", FileMode.Open, FileAccess.Read, FileShare.Read);

        var mediaFile = new MediaFile
        {
            OriginalFilename = "VietBun.jpg",
            Name = "Nice lunch",
            Description = "Correctly rotated",

            // Acquisition date is read from Date taken Exif tag if not explicitly specified
            AcquisitionDate = new DateTimeOffset(2023, 10, 01, 12, 15, 00, TimeSpan.FromHours(2)),

            // Rotation is read from Exif tags if not explicitly specified
            Rotation = 90,
        };

        // Upload in one go as multipart form data
        var multiPartContent = new MultipartFormDataContent
        {
            HttpContentFactory.CreateBody(mediaFile),
            HttpContentFactory.CreateBody(fileStream, "image/jpeg")
        };

        // The only thing we need to do is specify the image id (must be of Guid type) in the upload Uri
        var link = study.Links.GetRequired(Rel.UploadMedia).AsTemplatedLink();
        Guid imageId = Guid.NewGuid();
        link.TrySetParameterValue("imageId", imageId.ToString());

        // Post file stream to the server
        var response = await _http.PostAsync(link.ToUrl(), multiPartContent, CancellationToken.None);
        await response.AssertSuccessAsync();

        // Deserialize the response body
        var createdImage = response.Deserialize<MediaFile>();

        Assert.AreEqual(imageId, createdImage.ImageId);
        Assert.AreEqual("image/jpeg", createdImage.ContentType);
        Assert.AreEqual(fileStream.Length, createdImage.FileSize);
        Assert.AreEqual(4032, createdImage.Width);
        Assert.AreEqual(3024, createdImage.Height);
        Assert.AreEqual(90, createdImage.Rotation);
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