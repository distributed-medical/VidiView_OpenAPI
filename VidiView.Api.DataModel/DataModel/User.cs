namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record User
{
    public static implicit operator IdAndName(User user)
    {
        return user == null ? null! : new IdAndName(user.Id, user.Name);
    }

    /// <summary>
    /// The internal VidiView ID of this user
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The user id authority
    /// </summary>
    /// <remarks>The issuer is either a well-known issuer (<see cref="WellKnownIssuerAuthority"/>)
    /// or an identifier of an external identity provider</remarks>
    public string Issuer { get; init; } = null!;

    /// <summary>
    /// Name of user id authority
    /// </summary>
    public string? IssuerName { get; init; }

    /// <summary>
    /// The authority's type of claim-value
    /// </summary>
    /// <remarks>The claim type is either a well-known claim type (<see cref="WellKnownClaimType"/>)
    /// or an identifier of an external claim type (for external identity providers)</remarks>
    public string ClaimType { get; init; } = null!;

    /// <summary>
    /// The authority's id of this user
    /// </summary>
    public string ClaimValue { get; init; } = null!;

    /// <summary>
    /// The user's name
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// The user's first name. This should be used 
    /// in Gui features to promote a friendly appearance.
    /// If null, use <see cref="Name"/> instead
    /// </summary>
    public string? FirstName { get; init; }

    public string? Initials { get; init; }

    public string? Email { get; init; }

    public string? Notes { get; init; }

    public string? AdministrativeId { get; init; }

    [Obsolete("Use Flags2 instead", false)]
    public long Flags { get; init; }

    public UserFlags Flags2 { get; init; }

    /// <summary>
    /// The user's access to entities in the system
    /// </summary>
    public ObjectAccess[]? AccessControlList { get; init; }

    /// <summary>
    /// The user's login name
    /// </summary>
    public string? Login { get; init; }

    /// <summary>
    /// Universal principal name
    /// </summary>
    public string? Upn { get; init; }

    public string DetermineLogin()
    {
        if (!string.IsNullOrEmpty(Login))
            return Login;
        else if (!string.IsNullOrEmpty(Upn))
            return Upn;
        else
            return ClaimType + " " + ClaimValue;
    }

    /// <summary>
    /// Time of last successful login
    /// </summary>
    public DateTimeOffset? LastLoginDate { get; init; }

    /// <summary>
    /// Number of invalid login attempts since last successful login
    /// </summary>
    public int InvalidLoginAttempts { get; init; }

    /// <summary>
    /// Any role membership the user has.
    /// </summary>
    [JsonPropertyName("_embedded")]
    public EmbeddedUserObjects? Embedded { get; init; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }

    public override string ToString() => Name;

}

[ExcludeFromCodeCoverage]
public record EmbeddedUserObjects
{
    public UserRole[]? Roles { get; init; }
}
