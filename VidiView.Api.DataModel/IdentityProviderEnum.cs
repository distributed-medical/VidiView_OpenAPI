namespace VidiView.Api.DataModel;

[Flags]
public enum IdentityProviderFlags
    : long
{
    None = 0x000,

    /// <summary>
    /// System defined identity provider
    /// </summary>
    SystemDefined = 0x001,

    /// <summary>
    /// Open ID Connect provider
    /// </summary>
    OidcProvider = 0x002,
}
