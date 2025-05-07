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
    public static Exception Factory(HttpStatusCode httpError, ProblemDetails problem, Uri? requestedUri)
    {
        // Check if we can instantiate the specific exception type
        try
        {
            if (!int.TryParse(problem.ErrorCode, out int errorCode))
                errorCode = 1000;

            var typeName = problem.Type.Replace(ProblemDetails.VidiViewExceptionUri, "VidiView.Api.Exceptions.");
            var type = Type.GetType(typeName, false, true);

            // Check if we have a local definition of this exception
            if (type != null)
            {
                var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                var constructor = type.GetConstructor(flags, new[] { typeof(string) });
                Debug.Assert(constructor != null, "Our exceptions are always supposed to have a constructor accepting a string");

                if (constructor != null)
                {
                    var exc = (Exception)constructor.Invoke(new[] { problem.Detail });
                    type.GetProperty(nameof(ErrorCode))?
                        .SetValue(exc, errorCode);
                    type.GetProperty(nameof(HttpStatusCode))?
                        .SetValue(exc, httpError);
                    type.GetProperty(nameof(Problem))?
                        .SetValue(exc, problem);
                    type.GetProperty(nameof(RequestedUri))?
                        .SetValue(exc, requestedUri);
                    type.GetProperty(nameof(ThrownServerSide))?
                        .SetValue(exc, true);

                    return exc;
                }
            }

            return new VidiViewException(problem?.Detail ?? $"{(int)httpError} {httpError}")
            {
                ErrorCode = errorCode,
                HttpStatusCode = httpError,
                Problem = problem,
                RequestedUri = requestedUri,
                ThrownServerSide = true
            };
        }
        catch
        {
            // Failed to create exception
            Debug.Assert(false);

            return new Exception($"Failed to instantiate corresponding exception class. Details {problem?.Detail ?? "<null>"}");
        }
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
    /// The raw problem details
    /// </summary>
    public ProblemDetails? Problem { get; init; }

    /// <summary>
    /// The requested URI that resulted in this error
    /// </summary>
    public Uri? RequestedUri { get; init; }

    /// <summary>
    /// True if the server was thrown on the server and deserialized client side
    /// </summary>
    public bool ThrownServerSide { get; init; }
}
