namespace VidiView.Api.DataModel;

public record Department
{
    public Guid Id { get; init; } 
    
    public Guid? ParentId { get; init; } 

    public string? AdministrativeId { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? InstitutionName { get; init; } 

    public string? InstitutionAddress { get; init; } 

    public string? SupportContactName { get; init; } 

    public string? SupportContactPhone { get; init; }

    /// <summary>
    /// This represents the permission set granted by the Server to this Department
    /// </summary>
    public long GrantedPermission { get; init; }

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
