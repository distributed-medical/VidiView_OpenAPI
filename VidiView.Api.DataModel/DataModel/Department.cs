namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record Department
{
    public static implicit operator IdAndName(Department department)
    {
        return department == null ? null! : new IdAndName(department.Id, department.Name);
    }

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
    /// The study types supported by this department
    /// </summary>
    public StudyType[]? StudyTypes { get; init; }

    /// <summary>
    /// The permissions granted to the current user for this department
    /// </summary>
    public long GrantedPermission { get; init; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }

    public override string ToString() => Name;
}
