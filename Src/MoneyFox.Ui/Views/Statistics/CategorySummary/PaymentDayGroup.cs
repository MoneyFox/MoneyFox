namespace MoneyFox.Ui.Views.Statistics.CategorySummary;

using Domain.Aggregates.AccountAggregate;

public class PaymentDayGroup : List<PaymentListItemViewModel>
{
    public PaymentDayGroup(DateOnly date, IEnumerable<PaymentListItemViewModel> payments) : base(payments)
    {
        Date = date;
    }

    public DateOnly Date { get; }

    public decimal TotalRevenue => this.Where(p => p.Type == PaymentType.Income).Sum(p => p.Amount);

    public decimal TotalExpense => this.Where(p => p.Type == PaymentType.Expense).Sum(p => p.Amount);
}
