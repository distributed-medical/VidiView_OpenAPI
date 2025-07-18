namespace VidiView.Api.Helpers;


public static class DateTimeExtension
{
    /// <summary>
    /// Format as a date-only parameter (yyyyMMdd), which is expected by the VidiView API
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    /// <remarks>Any time-of-day data is lost</remarks>
    public static string ToDateParameter(this DateTime dateTime)
    {
        return dateTime.ToString("yyyyMMdd");
    }

    /// <summary>
    /// Format as a date-only parameter (yyyyMMdd), which is expected by the VidiView API
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    /// <remarks>Any time-of-day data is lost</remarks>
    public static string? ToDateParameter(this DateTime? dateTime)
    {
        if (dateTime == null)
            return null;
        return dateTime.Value.ToString("yyyyMMdd");
    }

    /// <summary>
    /// Format as a date-only parameter (yyyyMMdd), which is expected by the VidiView API
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string ToDateParameter(this DateOnly dateTime)
    {
        return dateTime.ToString("yyyyMMdd");
    }

    /// <summary>
    /// Format as a date-only parameter (yyyyMMdd), which is expected by the VidiView API
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string? ToDateParameter(this DateOnly? dateTime)
    {
        if (dateTime == null)
            return null;
        return dateTime.Value.ToString("yyyyMMdd");
    }

}
