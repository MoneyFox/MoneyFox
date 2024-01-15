namespace MoneyFox.Ui.Views.Budget.BudgetOverview;

public class PaymentDayGroup(DateOnly date, List<BudgetPaymentViewModel> payments) : List<BudgetPaymentViewModel>(payments)
{
    public DateOnly Date { get; } = date;
}
