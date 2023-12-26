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

    public long? EffectivePermissionSet { get; init; }

    public WorklistType WorklistType { get; init; } = WorklistType.None;

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection Links { get; init; }

    public override string ToString() => Name;
}
