namespace VidiView.Api.Access.Headers;

/// <summary>
/// Add this header to request the server to pseudonymize patient information
/// </summary>
/// <remarks>
/// This header does not have any value
/// </remarks>
public class PseudonymizeHeader
{
    /// <summary>
    /// The header name
    /// </summary>
    public const string Name = "Pseudonymize";
}
