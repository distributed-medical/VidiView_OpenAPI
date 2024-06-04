using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidiView.Api.Helpers;
public class PreauthenticateRequired : IConnectState
{
    /// <summary>
    /// OpenID connect authentication required
    /// </summary>
    public const string Oidc = "oidc";

    public PreauthenticateRequired(string idp, Uri redirectUri, Uri apiUri, List<Uri> redirectHistory)
    {
        IdP = idp;
        RedirectUri = redirectUri;
        ApiUri = apiUri;
        RedirectHistory = redirectHistory;
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
    /// The uri to the Api
    /// </summary>
    internal Uri ApiUri { get; }

    /// <summary>
    /// Redirections processed to reach this result
    /// </summary>
    internal List<Uri> RedirectHistory { get; }

}
