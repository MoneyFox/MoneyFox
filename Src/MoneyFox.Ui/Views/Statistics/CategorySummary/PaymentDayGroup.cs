namespace MoneyFox.Ui.Views.Statistics.CategorySummary;

public class PaymentDayGroup : List<PaymentListItemViewModel>
{
    public PaymentDayGroup(DateOnly date, List<PaymentListItemViewModel> payments) : base(payments)
    {
        Date = date;
    }

    public DateOnly Date { get; }
}
