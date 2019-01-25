namespace MoneyFox.ServiceLayer
{
    public class StatisticEntry
    {
        public StatisticEntry(float value)
        {
            Value = value;
        }

        public float Value { get; }
        public string Label { get; set; }
        public string ValueLabel { get; set; }
        public string Color { get; set; }
        public string TextColor { get; set; }
    }
}
