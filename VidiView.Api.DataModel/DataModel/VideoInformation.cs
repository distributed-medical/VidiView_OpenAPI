using System.Text;

namespace VidiView.Api.DataModel;

public record VideoInformation
{
    /// <summary>
    /// Video width
    /// </summary>
    public int Width { get; init; }

    /// <summary>
    /// Video height
    /// </summary>
    public int Height { get; init; }

    /// <summary>
    /// Video frame rate (frames per second)
    /// </summary>
    public double? FrameRate { get; init; }

    /// <summary>
    /// The target bitrate (bits / second)
    /// </summary>
    public int? Bitrate { get; init; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        if (Width == 1280 && Height == 720)
            sb.Append("720p");
        else if (Width == 1920 && Height == 1080)
            sb.Append("1080p");
        else if (Width == 3840 && Height == 2160)
            sb.Append("2160p");
        else
            sb.Append($"{Width}x{Height}");

        if (FrameRate > 0)
        {
            if (FrameRate * 100 == Math.Floor(FrameRate.Value) * 100)
                sb.Append($"@{FrameRate:0}Hz"); // No decimals
            else
                sb.Append($"@{FrameRate:0.00}Hz");
        }

        if (Bitrate > 0)
        {
            if (Bitrate < 1000000)
                sb.Append($" ({Bitrate / 1000D:0}Kbit/s)");
            else
                sb.Append($" ({Bitrate / 1000000D:0.0}Mbit/s)");
        }

        return sb.ToString();
    }
}
