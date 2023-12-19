namespace VidiView.Api.DataModel;

public record struct IdAndName
{
    public Guid Id { get; init; }
    public string? Name { get; init; }

    public override string ToString()
    {
        return Name != null ? $"{Name} ({Id})" : Id.ToString();
    }
}
