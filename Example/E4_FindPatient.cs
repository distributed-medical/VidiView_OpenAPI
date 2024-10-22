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
            IdNumber = "197603221239"
        };

        var result = await _http.PostAsync(link, criteria);
        await result.AssertSuccessAsync();
        var patients = result.Deserialize<PatientCollection>();

        Assert.IsTrue(patients.Count >= 1);
        Assert.IsTrue(patients.Items[0].Name.HatFormat.StartsWith("Lundmark"));
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
        var result = await _http.PostAsync(link, new PatientSearch { IdNumber = "197604011234" });
        await result.AssertSuccessAsync();
        var patients = result.Deserialize<PatientCollection>();

        link = patients.Items.First().Links.GetRequired(Rel.Studies);
        var tl = link.AsTemplatedLink();
        tl.TrySetParameterValue("departmentId", "3b8d2e01-3f9e-4061-9ca5-8eb435d2b071");
        tl.TrySetParameterValue("fromDate", "20230101");

        var studies = await _http.GetAsync<StudyCollection>(tl);

        Assert.IsTrue(studies.Count > 0);

    }
}