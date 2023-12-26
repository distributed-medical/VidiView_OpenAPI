namespace VidiView.Api.DataModel;

/// <summary>
/// This class defines equipment that may have been used during a study
/// </summary>
public record Equipment
{
    /// <summary>
    /// Id of the unique equipment 
    /// </summary>
    public Guid EquipmentId { get; init; }

    /// <summary>
    /// The department in which this equipment is defined
    /// </summary>
    public IdAndName Department { get; init; }

    /// <summary>
    /// Equipment identification (serial-number / bar-code etc)
    /// </summary>
    public string Identification { get; init; } = string.Empty;

    /// <summary>
    /// Optional description
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Optional manufacturer
    /// </summary>
    public string? Manufacturer { get; init; }

    /// <summary>
    /// Optional model
    /// </summary>
    public string? Model { get; init; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection Links { get; set; }

    public override string ToString()
    {
        return Identification;
    }
}
