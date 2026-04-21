namespace VidiView.Api.Exceptions;

public class E1005_UnsupportedVersionException : VidiViewException
{
    public E1005_UnsupportedVersionException(string message)
        : base(1005, message)
    {
    }

    public string? MinimumSupportedVersion { get; init; }
}
