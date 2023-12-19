namespace VidiView.Api.DataModel;

public record PatchGuid
{
    public Guid OldValue { get; init; }
    public Guid NewValue { get; init; }

    public override string ToString()
    {
        return $"{OldValue} => {NewValue}";
    }
}
