using System.Diagnostics;
using System.Web;

namespace VidiView.Api.Helpers;
public class PreauthenticateRequired : IConnectState
{
    /// <summary>
    /// OpenID connect authentication required
    /// </summary>
    public const string Oidc = "oidc";

    public PreauthenticateRequired(string idp, Uri redirectUri, Uri requestedUri, List<Uri> callHistory)
    {
        IdP = idp;
        RedirectUri = redirectUri;
        RequestedUri = requestedUri;
        CallHistory = callHistory;

        if (idp == Oidc)
        {
            DetermineOidcParameters();
        }
    }

    /// <summary>
    /// The type of IdP indicated
    /// </summary>
    public string IdP { get; }

    /// <summary>
    /// The redirect Uri 
    /// </summary>
    public Uri RedirectUri { get; }

    /// <summary>
    /// The requested uri to the Api
    /// </summary>
    public Uri RequestedUri { get; }

    /// <summary>
    /// Uris processed to reach this result
    /// </summary>
    internal List<Uri> CallHistory { get; }

    public Uri? AuthEndPoint { get; private set; }
    public string? ClientId { get; private set; }
    public string? Scope { get; private set; }
    public string? Prompt { get; private set; }

    private void DetermineOidcParameters()
    {
        try
        {
            // Strip query string parameters for the endpoint
            var ub = new UriBuilder(RedirectUri);
            ub.Query = null;
            AuthEndPoint = ub.Uri;

            var queryParameters = HttpUtility.ParseQueryString(RedirectUri.Query);
            ClientId = queryParameters["client_id"];
            Scope = queryParameters["scope"];
            Prompt = queryParameters["prompt"];
        }
        catch
        {
            Debug.Assert(false);
        }
    }
}
