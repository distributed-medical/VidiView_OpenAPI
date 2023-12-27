namespace VidiView.Api.DataModel;

public record PatchString
{
    public string? OldValue { get; init; }
    public string? NewValue { get; init; }

    public override string ToString()
    {
        return $"{OldValue ?? "<null>"} => {NewValue ?? "<null>"}";
    }
}
