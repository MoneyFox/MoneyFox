namespace MoneyFox.Ui.Views.Budget.BudgetOverview;

public class PaymentDayGroup : List<BudgetPaymentViewModel>
{
    public PaymentDayGroup(DateOnly date, List<BudgetPaymentViewModel> payments) : base(payments)
    {
        Date = date;
    }

    public DateOnly Date { get; }
}
