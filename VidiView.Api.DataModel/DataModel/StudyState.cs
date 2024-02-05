namespace VidiView.Api.DataModel;
public enum StudyState
{
    Unknown = 0,

    /// <summary>
    /// Study is new and has no meaningful meta data
    /// </summary>
    New = 1,

    /// <summary>
    /// Study is prepared and is ready for media
    /// </summary>
    Prepared = 2,

    /// <summary>
    /// Study has received at least one media file
    /// </summary>
    Unreviewed = 3,

    /// <summary>
    /// Study is currently under review by a user
    /// </summary>
    UnderReview = 4,

    /// <summary>
    /// Study has been reviewed by a user
    /// </summary>
    Completed = 5,
}
