using VidiView.Api.DataModel;
using VidiView.Api.Helpers;
using VidiView.Example.TestData;

namespace VidiView.Example;

/// <summary>
/// This class demonstrates different ways of creating studies
/// </summary>
[TestClass]
public class E5_CreateStudy
{
    static HttpClient _http = null!;

    [ClassInitialize]
    public static async Task ClassInitialize(TestContext context)
    {
        _http = await HttpClientFactory.CreateAsync();
    }

    /// <summary>
    /// Create an unidentified study in the system.
    /// </summary>
    /// <remarks>Unidentified studies does not contain any patient identifier</remarks>
    /// <returns></returns>
    [TestMethod]
    public async Task Create_Unidentified_Study()
    {
        var api = await _http.HomeAsync();

        // Normally, we know which department the study should be created
        // in, but in this example we create it in the first possible department

        var departments = await _http.GetAsync<DepartmentCollection>(api.Links.GetRequired(Rel.Departments));
        var department = departments.Items.FirstOrDefault((d) => d.Links.Exists(Rel.CreateStudy));
        Assert.IsNotNull(department, "You are not granted Create study rights in any department");

        var link = department.Links.GetRequired(Rel.CreateStudy);

        // Generate a unique accession number
        string accessionNumber = "T" + DateTime.UtcNow.Ticks.ToString(); 
        if (accessionNumber.Length > 16) // Max length is 16 chars
            accessionNumber = accessionNumber.Substring(accessionNumber.Length - 16);

        // Initial parameters to assign to this study
        var sco = new StudyCreateOptions
        {
            AccessionNumber = accessionNumber,
            Patient = null
        };

        var response = await _http.PostAsync(link, sco);
        await response.AssertSuccessAsync();
        var study = response.Deserialize<Study>();

        // Now we have a study
        Assert.AreEqual(accessionNumber, study.AccessionNumber);
        Assert.IsNull(study.Patient, "Unidentified study does not contain any patient data");
        Assert.AreNotEqual(Guid.Empty, study.StudyId, "A unique id has been generated");
        Assert.AreEqual(DateTime.Today, study.StudyDate.LocalDateTime.Date, "The study date was set to current time on the VidiView Server");
    }
}