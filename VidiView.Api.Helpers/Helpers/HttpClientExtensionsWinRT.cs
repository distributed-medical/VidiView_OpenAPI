#if WINRT
using VidiView.Api.Headers;
using Windows.Web.Http;

namespace VidiView.Api.Helpers;

public static class HttpClientExtensionsWinRT
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
            if (!http.DefaultRequestHeaders.ContainsKey(PseudonymizeHeader.Name))
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
#endif
