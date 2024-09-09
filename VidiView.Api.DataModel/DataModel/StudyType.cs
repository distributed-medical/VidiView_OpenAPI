namespace VidiView.Api.DataModel;
public record StudyType
{
    /// <summary>
    /// Study type ID
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Internal type name
    /// </summary>
    public string Type { get; init; }

    /// <summary>
    /// The localized study type name
    /// </summary>
    public string? Name { get; init; }
}
