using System.Diagnostics;
using System.Net;
using System.Reflection;
using VidiView.Api.DataModel;

namespace VidiView.Api.Exceptions;

/// <summary>
/// This is the base VidiView exception class
/// </summary>
public class VidiViewException : Exception
{
    /// <summary>
    /// Instantiate a VidiView exception from the problem details received
    /// </summary>
    /// <param name="problem"></param>
    /// <returns></returns>
    public static Exception Factory(HttpStatusCode httpError, ProblemDetails problem)
    {
        // Check if we can instantiate the specific exception type
        if (!int.TryParse(problem.ErrorCode, out int errorCode))
            errorCode = 1000;

        var typeName = problem.Type.Replace(ProblemDetails.VidiViewExceptionUri, "VidiView.Api.Exceptions.");
        var type = Type.GetType(typeName, false, true);
        if (type != null)
        {
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var ci = type.GetConstructor(flags, new[] { typeof(string) });
            Debug.Assert(ci != null, "Our exceptions are always supposed to have a constructor accepting a string");

            if (ci != null)
            {
                var exc = (Exception)ci.Invoke(new[] { problem.Detail });
                type.GetProperty(nameof(ErrorCode))?
                    .SetValue(exc, errorCode);
                type.GetProperty(nameof(HttpStatusCode))?
                    .SetValue(exc, httpError);
                return exc;
            }
        }

        return new VidiViewException(problem?.Detail ?? $"{(int)httpError} {httpError}")
        {
            ErrorCode = errorCode,
            HttpStatusCode = httpError
        };
    }

    public VidiViewException(string message)
        : base(message)
    {
    }

    protected VidiViewException(string message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// VidiView error code
    /// </summary>
    public int ErrorCode { get; init; }

    /// <summary>
    /// The corresponding Http status code
    /// </summary>
    public HttpStatusCode HttpStatusCode { get; init; }

    /// <summary>
    /// The requested URI that resulted in this error
    /// </summary>
    public Uri? RequestedUri { get; set; }
}
