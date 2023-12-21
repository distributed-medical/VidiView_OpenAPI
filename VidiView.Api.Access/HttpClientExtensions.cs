using VidiView.Api.Access.Headers;

namespace VidiView.Api.Access;

public static class HttpClientExtensions
{
    /// <summary>
    /// Enable or disable API level pseudonymization 
    /// </summary>
    /// <param name="http"></param>
    /// <param name="enabled"></param>
    public static void EnablePseudonymization(this HttpClient http, bool enabled)
    {
        if (enabled)
        {
            if (!http.DefaultRequestHeaders.Contains(PseudonymizeHeader.Name))
            {
                http.DefaultRequestHeaders.Add(PseudonymizeHeader.Name, (string?) null);
            }
        }
        else
        {
            http.DefaultRequestHeaders.Remove(PseudonymizeHeader.Name);
        }
    }
}
