namespace VidiView.Api.DataModel;

[JsonConverter(typeof(StringEnumConverterEx<ADAccountType>))]
[Flags]
public enum ADAccountType
{
    User = 0x1,
    Group = 0x2
}

public record ADAccountObject
{
    /// <summary>
    /// The object type
    /// </summary>
    public ADAccountType ObjectType { get; init; }

    /// <summary>
    /// The organizational unit (container) of the object
    /// </summary>
    public string? OrganizationalUnit { get; init; }

    public string Sid { get; init; }

    /// <summary>
    /// The SAM account name
    /// </summary>
    public string? SamAccountName { get; init; }

    /// <summary>
    /// User principal name (UPN)
    /// </summary>
    public string? PrincipalName { get; init; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Description
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Email address
    /// </summary>
    public string? EmailAddress { get; init; }

    /// <summary>
    /// The account disabled state
    /// </summary>
    public bool AccountDisabled { get; init; }

    public override string ToString()
    {
        return $"{Name} ({SamAccountName ?? Sid})"; ;
    }
}
