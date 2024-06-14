using System.Text.Json;
using VidiView.Api.DataModel;
using VidiView.Api.Exceptions;
using VidiView.Api.Serialization;

namespace VidiView.Api.Helpers.Test.Exceptions;


[TestClass]
public class VidiViewExceptionTest
{
    [TestMethod]
    public void Create_From_ProblemDetails()
    {
        var problem = new ProblemDetails
        {
            Type = "http://schema.vidiview.com/exception/E1034_ConcurrentUpdateException",
            Title = "Save patient failed",
            Detail = "Concurrent update",
            ErrorCode = "1034",
        };

        var exc = VidiViewException.Factory(System.Net.HttpStatusCode.Conflict, problem);

        Assert.IsInstanceOfType<E1034_ConcurrentUpdateException>(exc);
        Assert.AreEqual("Concurrent update", exc.Message);
    }

    [TestMethod]
    public void Deserialize_AdditionalExceptionFields()
    {
        // Arrange - imitate the code in AssertNotProblem
        string problemJson = "{\"type\":\"http://schema.vidiview.com/exception/E1038_FieldRequiredException\",\"title\":\"Error 1038\",\"detail\":\"Field AccessionNumber is required\",\"description\":\"Field AccessionNumber is required\",\"error-code\":\"1038\",\"field-level\":\"Study\"}";
        var problem = JsonSerializer.Deserialize<ProblemDetails>(problemJson, VidiViewJson.DefaultOptions);
        problem.RawResponse = problemJson;

        var success = problem.TryGetPropertyValue<string>("FieldLevel", VidiViewJson.DefaultOptions, out var value);
        Assert.IsTrue(success);
        Assert.AreEqual("Study", value);
    }

    [TestMethod]
    public void Deserialize_Exception()
    {
        // Arrange - imitate the code in AssertNotProblem
        string problemJson = "{\"type\":\"http://schema.vidiview.com/exception/E1038_FieldRequiredException\",\"title\":\"Error 1038\",\"detail\":\"Field AccessionNumber is required\",\"description\":\"Field AccessionNumber is required\",\"error-code\":\"1038\",\"field-level\":\"Study\"}";
        var problem = JsonSerializer.Deserialize<ProblemDetails>(problemJson, VidiViewJson.DefaultOptions);
        problem.RawResponse = problemJson;

        var exc = VidiViewException.Factory(System.Net.HttpStatusCode.Conflict, problem);
        Assert.IsInstanceOfType<E1038_FieldRequiredException>(exc);
        Assert.AreEqual("Study", ((E1038_FieldRequiredException)exc).FieldLevel);
    }
}
