namespace MoneyFox.Ui.Views.Payments.PaymentList;

public class PaymentDayGroup : List<PaymentListItemViewModel>
{
    public PaymentDayGroup(DateOnly date, List<PaymentListItemViewModel> payments) : base(payments)
    {
        Date = date;
    }

    public DateOnly Date { get; }

    public decimal Sum => this.Sum(p => p.Amount);
}
