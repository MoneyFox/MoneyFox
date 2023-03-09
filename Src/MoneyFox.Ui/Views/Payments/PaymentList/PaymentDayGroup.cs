namespace MoneyFox.Ui.Views.Payments.PaymentList;

public class PaymentDayGroup : List<PaymentListItemViewModel>
{
    public DateOnly Date { get; }

    public string DateString => Date.ToString("dd.MMM");

    public PaymentDayGroup(DateOnly date, List<PaymentListItemViewModel> payments) : base(payments)
    {
        Date = date;
    }
}
