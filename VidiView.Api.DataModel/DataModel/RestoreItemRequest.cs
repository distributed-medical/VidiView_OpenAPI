namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record RestoreItemRequest
{
    public IdAndName Queue { get; init; }

    public IdAndName RequestedBy { get; init; }

    public DateTimeOffset RequestedDate { get; init; }

    /// <summary>
    /// Id of item being received
    /// </summary>
    public string SopInstanceUid { get; init; }

    public RestoreStatus Status { get; init; }

    public string? StatusDescription { get; init; }

    /// <summary>
    /// Number of bytes received so far
    /// </summary>
    public long BytesReceived { get; init; }

    /// <summary>
    /// Current transfer progress (if known)
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public float? Progress { get; init; }

    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }

}
