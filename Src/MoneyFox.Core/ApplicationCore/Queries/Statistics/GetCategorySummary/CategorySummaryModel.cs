namespace MoneyFox.Core.ApplicationCore.Queries.Statistics.GetCategorySummary;

using System.Collections.Generic;

public class CategorySummaryModel
{
    public CategorySummaryModel(decimal totalEarned, decimal totalSpent, List<CategoryOverviewItem> categoryOverviewItems)
    {
        TotalEarned = totalEarned;
        TotalSpent = totalSpent;
        CategoryOverviewItems = categoryOverviewItems;
    }

    public int CategoryId { get; set; }

    public decimal TotalEarned { get; set; }

    public decimal TotalSpent { get; set; }

    public List<CategoryOverviewItem> CategoryOverviewItems { get; }
}
