namespace VidiView.Api.DataModel;

public record CodedValue
{
    public string Schema { get; init; } = string.Empty;

    public string Version { get; init; } = string.Empty;

    public string Code { get; init; } = string.Empty;

    public string? Meaning { get; init; }

    public string? Comment { get; init; }

    public override string ToString() => $"{Code} {Meaning}".Trim();

    public virtual bool Equals(CodedValue? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        return Code == other.Code
            && Schema == other.Schema
            && Version == other.Version;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
