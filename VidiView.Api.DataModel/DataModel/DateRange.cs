namespace VidiView.Api.DataModel;

/// <summary>
/// This record represents a date range 
/// </summary>
/// <remarks>Only used for date range excluding time information</remarks>
public record DateRange 
{
    public DateRange()
    { }

    public DateRange(DateTime singleDate)
    {
        FromDate = singleDate;
        ToDate = singleDate;
    }

    public DateRange(DateTime? fromDate, DateTime? toDate)
    {
        if (fromDate > toDate)
        {
            // Invert
            ToDate = fromDate;
            FromDate = toDate;
        }
        else
        {
            FromDate = fromDate;
            ToDate = toDate;
        }
    }

    /// <summary>
    /// From date (inclusive)
    /// </summary>
    public DateTime? FromDate { get; init; }

    /// <summary>
    /// To date (inclusive)
    /// </summary>
    public DateTime? ToDate { get; init; }

}
