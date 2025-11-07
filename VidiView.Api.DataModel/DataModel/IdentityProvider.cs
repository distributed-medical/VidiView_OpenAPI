namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record IdentityProvider
{
    /// <summary>
    /// The friendly name
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// The identity provider id
    /// </summary>
    public string Issuer { get; init; } = null!;

    /// <summary>
    /// The authority URL
    /// </summary>
    public string AuthorityUrl { get; init; } = null!;

    /// <summary>
    /// The client id
    /// </summary>
    public string ClientId { get; init; } = null!;

    /// <summary>
    /// The scopes that should be requested from the identity provider
    /// </summary>
    public string RequestedScopes { get; init; } = null!;

    /// <summary>
    /// Optional prompt value to supply
    /// </summary>
    public string? Prompt { get; init; }

    /// <summary>
    /// The redirect URL
    /// </summary>
    public string RedirectUrl { get; init; } = null!;

    /// <summary>
    /// Flags
    /// </summary>
    public IdentityProviderFlags Flags { get; init; }

    public override string ToString()
    {
        return $"{Name} ({Issuer})";
    }

    [Obsolete("Not used anymore", true)]
    public bool SkipIdPLogout => Flags.HasFlag(IdentityProviderFlags.SkipIdPLogout);

    public bool PrivateSession => Flags.HasFlag(IdentityProviderFlags.PrivateSession);

}
