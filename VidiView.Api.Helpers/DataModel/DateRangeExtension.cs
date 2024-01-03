namespace VidiView.Api.DataModel;
public static class DateRangeExtension
{
    /// <summary>
    /// Returns true if the date range is null or conatin null/min values
    /// </summary>
    /// <param name="dateRange"></param>
    /// <returns></returns>
    public static bool IsEmpty(this DateRange? dateRange)
    {
        return IsNullOrEmpty(dateRange?.FromDate) && IsNullOrEmpty(dateRange?.ToDate);
    }

    /// <summary>
    /// Returns true if FromDate and ToDate is the same date
    /// </summary>
    /// <param name="dateRange"></param>
    /// <returns></returns>
    public static bool IsSingleDate(this DateRange? dateRange)
    {
        return !dateRange.IsEmpty() && dateRange!.FromDate == dateRange.ToDate;
    }

    static bool IsNullOrEmpty(DateTime? date)
    {
        return date == null || date == DateTime.MinValue;
    }
}
