namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record Department
{
    /// <summary>
    /// Department id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The department's parent id in the tree. Null if root level
    /// </summary>
    public Guid? ParentId { get; init; } 

    /// <summary>
    /// Optional administrative id
    /// </summary>
    public string? AdministrativeId { get; init; }

    /// <summary>
    /// Department name
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Hierarchy level and sort order
    /// </summary>
    /// <remarks>
    /// This is a hierarchy with sorting on each tree level. This is not set by all server versions
    /// </remarks>
    /// <example>
    /// 0
    /// 0/0
    /// 0/0/0
    /// 0/0/1
    /// 0/0/2
    /// 0/1
    /// 0/1/0
    /// 0/1/3
    /// 0/4
    /// 0/6
    /// 0/6/0
    /// 0/6/0/0
    /// </example>
    public string? Hierarchy { get; init; }

    public string? InstitutionName { get; init; } 

    public string? InstitutionAddress { get; init; } 

    public string? SupportContactName { get; init; } 

    public string? SupportContactPhone { get; init; }

    public WorklistType WorklistType { get; init; } = WorklistType.None;

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }

    public override string ToString() => Name;

    public static explicit operator IdAndName?(Department? department)
    {
        return department == null ? null : new IdAndName
        {
            Id = department.Id,
            Name = department.Name
        };
    }
}
