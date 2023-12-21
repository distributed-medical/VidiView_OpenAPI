namespace VidiView.Api.DataModel;

public record RecentlyUsedStudy : Study
{
    /// <summary>
    /// The time this study was last reviewed by the current user
    /// </summary>
    public DateTimeOffset LastReviewDate { get; init; }
}