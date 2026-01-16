namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record MediaType
{
    public static implicit operator IdAndName?(MediaType? type)
    {
        return type == null ? null : new IdAndName(type.Id, type.Name);
    }

    /// <summary>
    /// Media type ID
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The display name of the media type
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
}
