namespace MoneyFox.Application.Statistics
{
    public class StatisticEntry
    {
        public StatisticEntry(float value)
        {
            Value = value;
        }

        public float Value { get; }

        public string Label { get; set; } = string.Empty;

        public string ValueLabel { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;
    }
}
