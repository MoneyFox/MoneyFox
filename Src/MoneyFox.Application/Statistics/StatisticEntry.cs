namespace MoneyFox.Application.Statistics
{
    public class StatisticEntry
    {
        public StatisticEntry(decimal value, string label = "", string valueLabel = "")
        {
            Value = value;
            Label = label;
            ValueLabel = valueLabel;
        }

        public decimal Value { get; }

        public string Label { get; set; } = string.Empty;

        public string ValueLabel { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;
    }
}
