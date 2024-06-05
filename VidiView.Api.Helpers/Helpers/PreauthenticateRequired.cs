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
    internal Uri RequestedUri { get; }

    /// <summary>
    /// Uris processed to reach this result
    /// </summary>
    internal List<Uri> CallHistory { get; }

}
