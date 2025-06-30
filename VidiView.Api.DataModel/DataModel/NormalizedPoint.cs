using System.Globalization;

namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record struct NormalizedPoint
{
    public NormalizedPoint()
    {
    }

    public NormalizedPoint(double x, double y)
    {
        X = x; 
        Y = y;
    }

    /// <summary>
    /// Normalized X-coordinate (0.0 - 1.0)
    /// </summary>
    public double X { get; init; }

    /// <summary>
    /// Normalized Y-coordinate (0.0 - 1.0)
    /// </summary>
    public double Y { get; init; }

    public override string ToString()
    {
        return string.Create(CultureInfo.InvariantCulture, $"({X:0.####}, {Y:0.####})");
    }
}
