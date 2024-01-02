using VidiView.Api.Headers;
#if WINRT
using Windows.Web.Http;
#else
using System.Net.Http;
#endif

namespace VidiView.Api.Helpers;

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
#if WINRT
            if (!http.DefaultRequestHeaders.ContainsKey(PseudonymizeHeader.Name))
#else
            if (!http.DefaultRequestHeaders.Contains(PseudonymizeHeader.Name))
#endif
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
