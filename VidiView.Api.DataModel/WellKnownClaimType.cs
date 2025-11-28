namespace VidiView.Api;

/// <summary>
/// These are the well-known claim types for user accounts in VidiView
/// </summary>
public static class WellKnownClaimType
{
    /// <summary>
    /// If the Issuer equals <see cref="WellKnownIssuerAuthority.ActiveDirectory"/>,
    /// this indicates the account is a user account
    /// </summary>
    public const string ActiveDirectoryUser = "UserSid";

    /// <summary>
    /// If the Issuer equals <see cref="WellKnownIssuerAuthority.ActiveDirectory"/>,
    /// this indicates the account is a group account
    /// </summary>
    public const string ActiveDirectoryGroup = "GroupSid";

    /// <summary>
    /// If the Issuer equals <see cref="WellKnownIssuerAuthority.VidiView"/>,
    /// this indicates the account is a user account
    /// </summary>
    public const string VidiViewUser = "Login";

    /// <summary>
    /// If the Issuer equals <see cref="WellKnownIssuerAuthority.VidiView"/>,
    /// this indicates the account is a user account with X509 certificate authentication
    /// </summary>
    public const string VidiViewUserX509 = "X509";

    /// <summary>
    /// If the Issuer equals <see cref="WellKnownIssuerAuthority.VidiView"/>,
    /// this indicates the account is the system account
    /// </summary>
    public const string SystemUser = "System";

    /// <summary>
    /// If the Issuer equals <see cref="WellKnownIssuerAuthority.VidiView"/>,
    /// this indicates the account is the system account
    /// </summary>
    public const string VidiViewController = "Controller";

    /// <summary>
    /// If the Issuer equals an external identity provider,
    /// this indicates the account is an external user
    /// </summary>
    public const string ExternalUser = "Subject";

}
