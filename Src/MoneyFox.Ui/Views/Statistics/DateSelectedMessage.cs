namespace MoneyFox.Ui.Views.Statistics;

public class DateSelectedMessage
{
    public DateSelectedMessage(DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    /// <summary>
    ///     The selected start date
    /// </summary>
    public DateTime StartDate { get; }

    /// <summary>
    ///     The selected end date
    /// </summary>
    public DateTime EndDate { get; }
}
