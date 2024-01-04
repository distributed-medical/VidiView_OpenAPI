namespace VidiView.Api.DataModel;

public record IdAndName
{
    public static implicit operator IdAndName(Guid id)
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

    public override string ToString()
    {
        return Name != null ? $"{Name} ({Id})" : Id.ToString();
    }
}
