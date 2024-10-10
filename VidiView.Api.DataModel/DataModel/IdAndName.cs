namespace VidiView.Api.DataModel;

public record IdAndName
{
    /// <summary>
    /// Create instance with only a specific Id
    /// </summary>
    /// <param name="id"></param>
    public static explicit operator IdAndName(Guid id)
    {
        return new IdAndName(id);
    }

    public IdAndName()
    {
    }

    public IdAndName(Guid id)
    {
        Id = id;
    }

    public IdAndName(Guid id, string? name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; init; }
    public string? Name { get; init; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }

    public override string ToString()
    {
        return Name != null ? $"{Name} ({Id})" : Id.ToString();
    }

    public virtual bool Equals(IdAndName? other)
    {
        return other?.Id == this.Id;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
