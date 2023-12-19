namespace VidiView.Api.DataModel;

public class Snomed
{
    public string Expression { get; init; }

    public string? Compact { get; init; }

    public string? Meaning { get; init; }

    public override bool Equals(object? obj)
    {
        if (obj is Snomed s)
        {
            return s.Expression?.Equals(Expression) == true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Expression?.GetHashCode() ?? 0;
    }
}