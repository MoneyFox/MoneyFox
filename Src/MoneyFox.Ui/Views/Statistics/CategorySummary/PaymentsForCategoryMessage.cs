namespace MoneyFox.Ui.Views.Statistics.CategorySummary;

public class PaymentsForCategoryMessage
{
    public PaymentsForCategoryMessage(int? categoryId, DateTime startDate, DateTime endDate)
    {
        CategoryId = categoryId;
        StartDate = startDate;
        EndDate = endDate;
    }

    public int? CategoryId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
