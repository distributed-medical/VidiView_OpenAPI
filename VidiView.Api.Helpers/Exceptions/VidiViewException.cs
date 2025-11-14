using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text.Json;
using VidiView.Api.DataModel;
using VidiView.Api.Serialization;

namespace VidiView.Api.Exceptions;

/// <summary>
/// This is the base VidiView exception class
/// </summary>
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
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
        problem ??= new ProblemDetails();

        try
        {
            if (!int.TryParse(problem.ErrorCode, out int errorCode))
            {
                errorCode = -1;
            }

            var typeName = problem.Type.Replace(ProblemDetails.VidiViewExceptionUri, "VidiView.Api.Exceptions.");
            var type = Type.GetType(typeName, false, true);

            IReadOnlyDictionary<string, JsonElement> props = DeserializeProperties(problem.RawResponse);

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
                    type.GetProperty(nameof(Properties))?
                        .SetValue(exc, props);

                    // Set all properties we can find
                    var additionalProps = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
                    foreach (var prop in additionalProps)
                    {
                        if (problem.TryGetPropertyValue(prop.PropertyType, prop.Name, VidiViewJson.DefaultOptions, out var value))
                        {
                            try
                            {
                                prop.SetValue(exc, value);
                            }
                            catch
                            {
                                Debug.Assert(false, $"Failed to set property {prop.Name} of type {prop.PropertyType}");
                            }
                        }
                    }

                    return exc;
                }
            }

            string message = string.IsNullOrWhiteSpace(problem?.Detail) ? $"{(int)httpError} {httpError}" : problem.Detail;
            return new VidiViewException(message)
            {
                ErrorCode = errorCode,
                HttpStatusCode = httpError,
                Problem = problem,
                RequestedUri = requestedUri,
                ThrownServerSide = true,
                Properties = props
            };
        }
        catch
        {
            // Failed to create exception
            Debug.Assert(false);

            return new Exception($"Failed to instantiate corresponding exception class. Details {problem?.Detail ?? "<null>"}");
        }
    }

    private static IReadOnlyDictionary<string, JsonElement> DeserializeProperties(string rawValue)
    {
        if (!string.IsNullOrEmpty(rawValue))
        {
            try
            {
                // Deserialize additional properties
                using var jdoc = JsonDocument.Parse(rawValue);
                var result = jdoc.RootElement.EnumerateObject()
                    .SelectMany(p => GetLeaves(null, p))
                    .ToDictionary(k => k.Path, v => v.P.Value.Clone()); //Clone so that we can use the values outside of using

                result.Remove("type");
                result.Remove("title");
                result.Remove("detail");
                result.Remove("description");
                result.Remove("error-code");
                return result;
            }
            catch
            {
                Debug.Assert(false);
            }
        }

        return new Dictionary<string, JsonElement>();
    }

    private static IEnumerable<(string Path, JsonProperty P)> GetLeaves(string? path, JsonProperty p)
    {
        path = (path == null) ? p.Name : path + "." + p.Name;
        if (p.Value.ValueKind != JsonValueKind.Object)
        {
            yield return (Path: path, P: p);
        }
        else
        {
            foreach (JsonProperty child in p.Value.EnumerateObject())
            {
                foreach (var leaf in GetLeaves(path, child))
                {
                    yield return leaf;
                }
            }
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

    /// <summary>
    /// Raw Json properties
    /// </summary>
    internal IReadOnlyDictionary<string, JsonElement>? Properties { get; init; }
}
