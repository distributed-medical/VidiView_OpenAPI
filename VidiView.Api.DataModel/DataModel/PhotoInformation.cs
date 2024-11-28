namespace VidiView.Api.DataModel;

public record PhotoInformation
{
    /// <summary>
    /// Photo width
    /// </summary>
    public int Width { get; init; }

    /// <summary>
    /// Photo height
    /// </summary>
    public int Height { get; init; }
}
