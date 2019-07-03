namespace MoneyFox.Application.Statistics.Queries.GetCategorySummary
{
    public class CategoryOverviewItem
    {
        public string Label { get; set; }
        public double Value { get; set; }
        public double Average { get; set; }
        public double Percentage { get; set; }
    }
}