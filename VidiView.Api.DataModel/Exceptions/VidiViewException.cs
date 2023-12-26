using System;
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
    /// Create a VidiView exception from the error details received
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static Exception Factory(HttpStatusCode httpError, ErrorDetails? error)
    {
        // Check if we can instantiate the specific exception type
        if (!int.TryParse(error?.ErrorCode, out int errorCode))
            errorCode = 1000;

        if (error != null)
        {
            var typeName = "VidiView.Api.DataModel.Exceptions." + error.Type;
            var type = Type.GetType(typeName, false, true);
            if (type != null)
            {
                var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                var ci = type.GetConstructor(flags, new[] { typeof(string) });
                if (ci != null)
                {
                    var inst = ci.Invoke(new[] { error.Description });

                    type.GetProperty(nameof(ErrorCode))?
                        .SetValue(inst, errorCode);

                    type.GetProperty(nameof(HttpStatusCode))?
                        .SetValue(inst, httpError);

                    return (Exception)inst;
                }
            }
        }

        return new VidiViewException(error?.Description ?? $"{(int)httpError} {httpError}")
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
