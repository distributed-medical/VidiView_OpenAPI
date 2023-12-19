namespace VidiView.Api.DataModel;

public class PersonalWorklist
{
    /// <summary>
    /// The id of this worklist
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User's name of this worklist
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// User's description of this worklist
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")] 
    public LinkCollection Links { get; set; }

    public override string ToString() => Name;
}
