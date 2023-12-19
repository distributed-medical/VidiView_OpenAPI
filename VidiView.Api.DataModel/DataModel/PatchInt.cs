namespace VidiView.Api.DataModel;
public record PatchInt
{
    public int? OldValue { get; init; }
    public int? NewValue { get; init; }

    public override string ToString()
    {
        return $"{OldValue?.ToString() ?? "<null>"} => {NewValue?.ToString() ?? "<null>"}";
    }
}