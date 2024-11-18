namespace VidiView.Api.DataModel;

/// <summary>
/// This is a subset of the flags above
/// </summary>
[JsonConverter(typeof(StringEnumConverterEx<UserFlags>))]
[Flags]
public enum UserFlags
    : long
{
    None = 0x0,

    /// <summary>
    /// Implicit user, added due to group membership
    /// </summary>
    Implicit = 0x0001,

    /// <summary>
    /// Global system administrator
    /// </summary>
    SysAdmin = 0x0004,

    /// <summary>
    /// This user may manage users
    /// </summary>
    UserAdmin = 0x0008,

    /// <summary>
    /// This user may review audit logs
    /// </summary>
    ReviewAuditLog = 0x0010,

    /// <summary>
    /// This user may only display anonymized information
    /// </summary>
    AlwaysPseudonymize = 0x0020,

    /// <summary>
    /// This user may manage patient information
    /// </summary>
    PatientAdmin = 0x0040,

    /// <summary>
    /// User password never expires (only for VidiView authentication)
    /// </summary>
    PasswordNeverExpires = 0x0100,

    /// <summary>
    /// Require password change on next logon (only for VidiView authentication)
    /// </summary>
    RequirePasswordChange = 0x0200,

    /// <summary>
    /// Disabled user
    /// </summary>
    Disabled = 0x1000,

    /// <summary>
    /// This user is actually a group
    /// </summary>
    IsGroupClaim = 0x2000,

}
