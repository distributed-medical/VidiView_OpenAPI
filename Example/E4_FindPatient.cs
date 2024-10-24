using VidiView.Api.DataModel;
using VidiView.Api.Helpers;
using VidiView.Example.TestData;

namespace VidiView.Example;

/// <summary>
/// This class demonstrates the process of finding a patient with
/// the connected VidiView Server.
/// Ensure that the device is registered and granted access and
/// that authentication succeeds in Step 2 before continuing with this step
/// </summary>
[TestClass]
public class E4_FindPatient
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
    public async Task Find_By_Id()
    {
        var start = await _http.HomeAsync();

        var link = start.Links.GetRequired(Rel.FindPatient);

        // To find patients, we use POST, to not reveal patient information in URL:s
        var criteria = new PatientSearch
        {
            IdNumber = TestConfig.PatientId
        };

        var result = await _http.PostAsync(link, criteria);
        await result.AssertSuccessAsync();
        var patients = result.Deserialize<PatientCollection>();

        Assert.IsTrue(patients.Count == 1);
        Assert.IsTrue(patients.Items[0].Name.HatFormat == "Strandqvist^Karin");
    }

    /// <summary>
    /// List studies in a specific department by date
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task List_Patient_Studies()
    {
        var start = await _http.HomeAsync();

        var link = start.Links.GetRequired(Rel.FindPatient);
        var result = await _http.PostAsync(link, new PatientSearch { IdNumber = TestConfig.PatientId });
        await result.AssertSuccessAsync();
        var patients = result.Deserialize<PatientCollection>();

        link = patients.Items.First().Links.GetRequired(Rel.Studies);
        var tl = link.AsTemplatedLink();
        tl.TrySetParameterValue("departmentId", TestConfig.DepartmentId.ToString());
        tl.TrySetParameterValue("fromDate", "20230101");

        var studies = await _http.GetAsync<StudyCollection>(tl);

        Assert.IsTrue(studies.Count > 2);

    }
}