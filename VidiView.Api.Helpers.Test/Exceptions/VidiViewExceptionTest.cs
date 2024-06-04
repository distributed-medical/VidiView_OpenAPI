using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidiView.Api.DataModel;
using VidiView.Api.Exceptions;

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
}
