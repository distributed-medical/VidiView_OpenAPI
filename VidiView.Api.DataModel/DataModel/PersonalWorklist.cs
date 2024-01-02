namespace VidiView.Api.DataModel;

public record PersonalWorklist
{
    /// <summary>
    /// The id of this worklist
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// User's name of this worklist
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// User's description of this worklist
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")] 
    public LinkCollection? Links { get; init; }

    public override string ToString() => Name;
}
