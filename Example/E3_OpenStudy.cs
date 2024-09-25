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
    public async Task List_By_Date()
    {
        var start = await _http.HomeAsync();

        // Retrieve all accessible departments
        var link = start.Links.GetRequired(Rel.Departments);
        var departments = await _http.GetAsync<DepartmentCollection>(link);

        // List studies in the first study we are granted List access
        var department = departments.Items.FirstOrDefault((d) => d.Links.Exists(Rel.Studies));
        Assert.IsNotNull(department, "You are not granted List access in any department");

        link = department.Links.GetRequired(Rel.Studies);

        // We must provide parameters to this call
        var tl = link.AsTemplatedLink();
        tl.TrySetParameterValue("fromDate", "20240901"); // September 1st 2024
        tl.TrySetParameterValue("toDate", "20240930"); // September 30th 2024
        var studies = await _http.GetAsync<StudyCollection>(tl);

        Assert.IsTrue(studies.Count > 0);
    }

    /// <summary>
    /// Load a study by a unique identifier 
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task Load_Study_By_Id()
    {
        var start = await _http.HomeAsync();

        // The ID parameter of a Find can be any identifier, such as:
        // VidiView ID, Study Instance UID, Accession number etc..
        var tl = start.Links.GetRequired(Rel.FindStudy).AsTemplatedLink();
        var success = tl.TrySetParameterValue("id", "0a743fda-6373-40d4-b1ed-d426e9b523cf");
        Assert.IsTrue(success);

        var studies = await _http.GetAsync<StudyCollection>(tl);
        Assert.IsTrue(studies.Count == 1);
        Assert.IsTrue(studies.Items[0].StudyId == new Guid("0a743fda-6373-40d4-b1ed-d426e9b523cf"));

        // Since the study is the result of a Find, it is not "opened"
        // and therefore we have limited data about it

        // Go ahead and open the study
        var link = studies.Items[0].Links.GetRequired(Rel.Self);
        var study = await _http.GetAsync<Study>(link);

        // Load media for this study as well
        link = study.Links.GetRequired(Rel.Files);
        var mediaFiles = await _http.GetAsync<MediaFileCollection>(link);
        Assert.IsTrue(mediaFiles.Count > 0);
    }

    /// <summary>
    /// Load a study by a unique identifier 
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task Load_Media_File_From_Specific_Study()
    {
        var start = await _http.HomeAsync();

        // The quickest way to open a study that we know the unique id for 
        // (must be VidiView ID or Study Instance UID which are always unique)
        var tl = start.Links.GetRequired(Rel.Study).AsTemplatedLink();
        tl.TrySetParameterValue("studyId", "0a743fda-6373-40d4-b1ed-d426e9b523cf");
        var study = await _http.GetAsync<Study>(tl);

        // Load media for this study as well
        var link = study.Links.GetRequired(Rel.Files);
        var mediaFiles = await _http.GetAsync<MediaFileCollection>(link);

        // Get thumbnails
        foreach (var mediaFile in mediaFiles.Items)
        {
            var hasThumbnail = mediaFile.Links.TryGet(Rel.Thumbnail, out link);
            if (hasThumbnail)
            {
                try
                {
                    // Download the thumbnail, which is normally a jpg
                    using HttpContentStream thumbnailStream = await _http.GetStreamAsync(link);
                    Assert.AreEqual("image/jpeg", thumbnailStream.ContentType);
                    Assert.IsTrue(thumbnailStream.Length > 0);
                }
                catch
                {
                    // Strange
                }
            }
        }

        // Download the first image
        var photo = mediaFiles.Items.FirstOrDefault((f) => f.ContentType == "image/jpeg");
        link = photo.Links.GetRequired(Rel.Enclosure);

        using var stream = await _http.GetStreamAsync(link);
        Assert.AreEqual("image/jpeg", stream.ContentType);
        Assert.IsTrue(stream.Length > 10000);

    }



}