namespace VidiView.Api;
public static class ApiVersion
{
    /// <summary>
    /// This is the minimum compatible Api version
    /// </summary>
    /// <remarks>
    /// If the server has an Api version lower than this value, 
    /// it should be treated as a major issue
    /// </remarks>
    public static Version MinimumServerApiVersion = new Version(5, 0);

    /// <summary>
    /// This is the version of the server Api that this library is tested against
    /// </summary>
    public static Version TestedApiVersion = new Version(5, 0);
}
