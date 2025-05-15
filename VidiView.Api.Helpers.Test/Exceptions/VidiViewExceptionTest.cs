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

        var exc = VidiViewException.Factory(System.Net.HttpStatusCode.Conflict, problem, null);

        Assert.IsInstanceOfType<E1034_ConcurrentUpdateException>(exc);
        Assert.AreEqual("Concurrent update", exc.Message);
    }

    [TestMethod]
    public void Create_From_Null()
    {
        var problem = new ProblemDetails();
        var exc = VidiViewException.Factory(System.Net.HttpStatusCode.Conflict, null!, null);

        Assert.IsInstanceOfType<VidiViewException>(exc);
        Assert.AreEqual(-1, ((VidiViewException)exc).ErrorCode);
        Assert.AreEqual("409 Conflict", exc.Message);
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

        var exc = VidiViewException.Factory(System.Net.HttpStatusCode.Conflict, problem, null);
        Assert.IsInstanceOfType<E1038_FieldRequiredException>(exc);
        Assert.AreEqual("Study", ((E1038_FieldRequiredException)exc).FieldLevel);
    }

    [TestMethod]
    public void Deserialize_Exception_With_Properties()
    {
        // Arrange - imitate the code in AssertNotProblem
        string problemJson = "{\"type\":\"http://schema.vidiview.com/exception/E9999_FutureException\",\"title\":\"Future error\",\"detail\":\"Very strange indeed\",\"error-code\":\"9999\",\"field-level\":\"Study\",\"field-index\":99,\"host\":{\"id\":\"8BF3DF82-EBB6-4A1A-89CA-7735A6DA767A\", \"name\":\"testhost\"}}";
        var problem = JsonSerializer.Deserialize<ProblemDetails>(problemJson, VidiViewJson.DefaultOptions);
        problem.RawResponse = problemJson;
        var exc = (VidiViewException)VidiViewException.Factory(System.Net.HttpStatusCode.InternalServerError, problem, null);

        Assert.AreEqual(9999, exc.ErrorCode);
        Assert.AreEqual("Study", exc.Properties["field-level"].GetString());
        Assert.AreEqual(99, exc.Properties["field-index"].GetInt32());
        Assert.AreEqual("8BF3DF82-EBB6-4A1A-89CA-7735A6DA767A", exc.Properties["host.id"].GetString());
        Assert.AreEqual("testhost", exc.Properties["host.name"].GetString());

        var expected = new IdAndName(Guid.Parse("8BF3DF82-EBB6-4A1A-89CA-7735A6DA767A"), "testhost");
        var success = exc.Problem.TryGetPropertyValue<IdAndName>("host", VidiViewJson.DefaultOptions, out var actual);

        Assert.IsTrue(success);
        Assert.AreEqual(expected, actual);

    }

}
