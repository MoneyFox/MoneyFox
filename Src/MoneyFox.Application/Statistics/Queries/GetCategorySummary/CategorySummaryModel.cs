using System.Collections.Generic;

namespace MoneyFox.Application.Statistics.Queries.GetCategorySummary
{
    public class CategorySummaryModel
    {
        public CategorySummaryModel(decimal totalEarned, decimal totalSpent, List<CategoryOverviewItem> categoryOverviewItems)
        {
            TotalEarned = totalEarned;
            TotalSpent = totalSpent;
            CategoryOverviewItems = categoryOverviewItems;
        }

        public decimal TotalEarned { get; set; }
        public decimal TotalSpent { get; set; }
        public List<CategoryOverviewItem> CategoryOverviewItems { get; set; }
    }
}
