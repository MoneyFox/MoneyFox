namespace MoneyFox.Ui.Views.Statistics;

public class DateSelectedMessage(DateTime startDate, DateTime endDate)
{
    public DateTime StartDate { get; } = startDate;

    public DateTime EndDate { get; } = endDate;
}
