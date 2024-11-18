namespace VidiView.Api.DataModel;

public record User
{
    // Well-known issuers
    public const string IdentityActiveDirectory = "http://schema.vidiview.com/2022/03/identity/claims/ad";
    public const string IdentityVidiView = "http://schema.vidiview.com/2022/03/identity/claims/vidiview";

    public const string DefaultUserClaimType = "Subject";

    /// <summary>
    /// The user id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The user id authority
    /// </summary>
    public string Issuer { get; init; } = null!;

    /// <summary>
    /// Name of user id authority
    /// </summary>
    public string? IssuerName { get; init; }

    /// <summary>
    /// The type of this claim
    /// </summary>
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

    public static explicit operator IdAndName?(User? user)
    {
        return user == null ? null : new IdAndName
        {
            Id = user.Id,
            Name = user.Name
        };
    }
}

public record EmbeddedUserObjects
{
    public UserRole[]? Roles { get; init; }
}
