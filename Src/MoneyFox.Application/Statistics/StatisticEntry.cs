namespace MoneyFox.Application.Statistics
{
    public class StatisticEntry
    {
        public StatisticEntry(decimal value)
        {
            Value = value;
        }

        public decimal Value { get; }

        public string Label { get; set; } = string.Empty;

        public string ValueLabel { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;
    }
}
