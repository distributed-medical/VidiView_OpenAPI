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

    public bool IsEmpty =>
        (FromDate == null || FromDate == DateTime.MinValue)
        && (ToDate == null || ToDate == DateTime.MinValue || ToDate == DateTime.MaxValue);

    public bool IsSingleDate => !IsEmpty && FromDate == ToDate;

    public override string ToString()
    {
        if (IsEmpty)
            return "{DateRange IsEmpty = True}";
        else if (IsSingleDate)
            return $"{{DateRange {FromDate?.ToShortDateString()}}}";
        else
            return $"{{DateRange {(FromDate?.ToShortDateString() ?? "-")} to {(ToDate?.ToShortDateString() ?? "-" )}}}";
    }
}
