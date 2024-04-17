namespace VidiView.Api.DataModel;

public record struct PointInt
{
    public PointInt()
    {
    }

    public PointInt(int x, int y)
    {
        X = x; 
        Y = y;
    }

    public int X { get; init; }
    public int Y { get; init; }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}
