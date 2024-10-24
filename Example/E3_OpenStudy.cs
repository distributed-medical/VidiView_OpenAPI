using VidiView.Example.HttpHandlers;
using VidiView.Example.TestData;
using VidiView.Api.Authentication;
using VidiView.Api.DataModel;
using VidiView.Api.Helpers;

namespace VidiView.Example;

/// <summary>
/// This class demonstrates the process of authenticating with
/// the connected VidiView Server.
/// Ensure that the device is registered and granted access and
/// that authentication succeeds in Step 2 before continuing with this step
/// </summary>
[TestClass]
public class E3_OpenStudy
{
    static HttpClient _http = null!;

    /// <summary>
    /// Connect to VidiView Server and authenticate
    /// </summary>
    /// <param name="context"></param>
    [ClassInitialize]
    public static async Task ClassInitialize(TestContext context)
    {
        _http = await HttpClientFactory.CreateAsync();
    }

    /// <summary>
    /// List studies in a specific department by date
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task List_Department_Studies_By_Date()
    {
        var start = await _http.HomeAsync();

        // Retrieve all accessible departments
        var link = start.Links.GetRequired(Rel.Departments);
        var departments = await _http.GetAsync<DepartmentCollection>(link);

        // List studies in the first study we are granted List access
        var department = departments.Items.FirstOrDefault((d) => d.Id == TestConfig.DepartmentId);
        Assert.IsNotNull(department, "Inaccessible department");

        link = department.Links.GetRequired(Rel.Studies);

        // We must provide parameters to this call
        var tl = link.AsTemplatedLink();
        tl.TrySetParameterValue("fromDate", "20240901"); // September 1st 2024
        tl.TrySetParameterValue("toDate", "20240930"); // September 30th 2024
        var studies = await _http.GetAsync<StudyCollection>(tl);

        Assert.IsTrue(studies.Count > 50);
    }

    /// <summary>
    /// Load a study by any unique identifier 
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task Find_Study_By_Unique_Identifier()
    {
        var start = await _http.HomeAsync();

        // The ID parameter of a Find can be any identifier, such as:
        // VidiView ID, Study Instance UID, Accession number etc..
        var tl = start.Links.GetRequired(Rel.FindStudy).AsTemplatedLink();
        var success = tl.TrySetParameterValue("id", TestConfig.StudyId);
        Assert.IsTrue(success);

        var studies = await _http.GetAsync<StudyCollection>(tl);
        Assert.IsTrue(studies.Count == 1);
        Assert.IsTrue(studies.Items[0].StudyInstanceUid == TestConfig.StudyId);

        // Since the study is the result of a Find, it is not "opened"
        // and therefore we have limited data about it

        // Go ahead and open the study
        var link = studies.Items[0].Links.GetRequired(Rel.Self);
        var study = await _http.GetAsync<Study>(link);

        // Load media information for this study as well
        link = study.Links.GetRequired(Rel.Files);
        var mediaFiles = await _http.GetAsync<MediaFileCollection>(link);
        Assert.IsTrue(mediaFiles.Count > 2);
    }

    /// <summary>
    /// Load a study by a unique identifier and download thumbnails for all media files
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task Download_Thumbnail_And_Media_File()
    {
        var start = await _http.HomeAsync();

        // The quickest way to open a study when we know the VidiView ID (or StudyInstanceUid)
        var tl = start.Links.GetRequired(Rel.Study).AsTemplatedLink();
        tl.TrySetParameterValue("studyId", TestConfig.StudyId);
        var study = await _http.GetAsync<Study>(tl);

        // Load media files for this study as well
        var link = study.Links.GetRequired(Rel.Files);
        var mediaFiles = await _http.GetAsync<MediaFileCollection>(link);

        // Get thumbnails
        int thumbnailCount = 0;
        foreach (var mediaFile in mediaFiles.Items)
        {
            // If a thumbnail is available, download id
            var hasThumbnail = mediaFile.Links.TryGet(Rel.Thumbnail, out link);
            if (hasThumbnail)
            {
                // Download the thumbnail, which is always jpeg
                using HttpContentStream thumbnailStream = await _http.GetStreamAsync(link);
                Assert.AreEqual("image/jpeg", thumbnailStream.ContentType);
                Assert.IsTrue(thumbnailStream.Length > 0);
                thumbnailCount++;
            }

            if (thumbnailCount > 5)
                break; // Point proven
        }
        Assert.IsTrue(thumbnailCount > 0);

        // Download the first jpeg image
        var photo = mediaFiles.Items.FirstOrDefault((f) => f.ContentType == "image/jpeg");
        link = photo.Links.GetRequired(Rel.Enclosure);

        using var stream = await _http.GetStreamAsync(link);
        Assert.AreEqual("image/jpeg", stream.ContentType);
        Assert.IsTrue(stream.Length > 10000);

    }



}