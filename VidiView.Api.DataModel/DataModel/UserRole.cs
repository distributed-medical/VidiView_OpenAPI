namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record UserRole
{
    /// <summary>
    /// The Role Id
    /// </summary>
    public string Id { get; init; } = null!;

    /// <summary>
    /// The role name
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Id of the role provider
    /// </summary>
    public Guid RoleProviderId { get; init; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }

    public override string ToString() => Name;
}
