namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record StudyType
{
    public static implicit operator IdAndName(StudyType type)
    {
        return type == null ? null! : new IdAndName(type.Id, type.Name);
    }

    /// <summary>
    /// Study type ID
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The display name of the study type
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Type code schema
    /// </summary>
    public string? Schema { get; init; }

    /// <summary>
    /// Type code schema version
    /// </summary>
    public string? Version { get; init; }

    /// <summary>
    /// Type code
    /// </summary>
    public string? Code { get; init; }

    /// <summary>
    /// Type code meaning
    /// </summary>
    public string? Meaning { get; init; }

    /// <summary>
    /// Type flags
    /// </summary>
    public TypeFlags Flags { get; init; }

    /// <summary>
    /// Supported media types
    /// </summary>
    /// <remarks>If <see cref="Flags"/> defines AcceptAnyMediaType, these media types 
    /// are considered recommendations, but any media type can be set</remarks>
    public MediaType[] MediaTypes { get; init; }

    /// <summary>
    /// The default media type that is assigned to media files
    /// </summary>
    public Guid? DefaultMediaType { get; set; }

    /// <summary>
    /// Supported anatomic maps
    /// </summary>
    public IdAndName[]? AnatomicMaps { get; init; }

    /// <summary>
    /// Supported modalities (as defined in DICOM)
    /// </summary>
    public string[]? Modalities { get; init; }
}
