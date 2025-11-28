namespace VidiView.Api;

/// <summary>
/// These are the defined issuers for user accounts in VidiView
/// </summary>
public static class WellKnownIssuerAuthority
{
    // Well-known issuers
    public const string ActiveDirectory = "http://schema.vidiview.com/2022/03/identity/claims/ad";
    public const string VidiView = "http://schema.vidiview.com/2022/03/identity/claims/vidiview";
}
