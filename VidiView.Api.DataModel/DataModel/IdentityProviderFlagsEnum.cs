namespace VidiView.Api.DataModel;


[JsonConverter(typeof(StringEnumConverterEx<IdentityProviderFlags>))]
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

    /// <summary>
    /// Specifies that each connection with the IdP should use 
    /// its own private session, and not cache single sign on
    /// cookies etc
    /// </summary>
    PrivateSession = 0x010,

    /// <summary>
    /// If set, the client should only throw away its access 
    /// token and not perform IdP logout flow
    /// </summary>
    SkipIdPLogout = 0x020,
}
