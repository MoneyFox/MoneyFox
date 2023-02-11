namespace MoneyFox.Core.Queries.Statistics.GetCategorySummary;

public class CategoryOverviewItem
{
    public int? CategoryId { get; set; }

    public string Label { get; set; } = "";

    public decimal Value { get; set; }

    public decimal Average { get; set; }

    public decimal Percentage { get; set; }
}
