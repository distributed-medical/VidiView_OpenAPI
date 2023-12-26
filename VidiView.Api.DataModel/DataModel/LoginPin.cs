namespace VidiView.Api.DataModel;

public record LoginPin
{
    /// <summary>
    /// Is pin code set/enabled for user
    /// </summary>
    public bool IsSet { get; init; } = false;

    /// <summary>
    /// If pin is enabled for user, this property will hold the user's name
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Minimum pin code length, if allowed
    /// </summary>
    public int? MinimumLength { get; init; }

    /// <summary>
    /// Pin code to set for user
    /// </summary>
    public string? PinCode { get; init; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection Links { get; init; }
}
