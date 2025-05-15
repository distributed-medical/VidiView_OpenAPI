namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public static class UserClaimType
{
    public const string ActiveDirectoryUser = "UserSid";
    public const string ActiveDirectoryGroup = "GroupSid";

    public const string VidiViewUser = "Login";
    public const string VidiViewUserX509 = "X509";
    public const string VidiViewController = "Controller";
    public const string System = "System";

    public const string ExternalUser = "Subject";
}