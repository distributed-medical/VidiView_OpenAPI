namespace VidiView.Api.DataModel;

public class PatchString
{
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }

    public override string ToString()
    {
        return $"{OldValue ?? "<null>"} => {NewValue ?? "<null>"}";
    }
}
