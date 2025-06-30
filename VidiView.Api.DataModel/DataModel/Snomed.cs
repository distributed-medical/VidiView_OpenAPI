namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record Snomed
{
    public Snomed()
    {
    }

    public Snomed(string expression)
    {
        Expression = expression;
    }

    /// <summary>
    /// The Snomed expression. This typically is a numeric code, but can also be a more complex expression.
    /// </summary>
    public string Expression { get; init; }

    /// <summary>
    /// Compact expression, including code name if available
    /// </summary>
    public string? Compact { get; init; }

    /// <summary>
    /// Verbose meaning of expression
    /// </summary>
    public string? Meaning { get; init; }
}