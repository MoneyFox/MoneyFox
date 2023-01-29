namespace MoneyFox.Core.Queries.Statistics;

public class StatisticEntry
{
    public StatisticEntry(decimal value, string label = "", string valueLabel = "")
    {
        Value = value;
        Label = label;
        ValueLabel = valueLabel;
    }

    public decimal Value { get; }

    public string Label { get; set; }

    public string ValueLabel { get; set; }

    public string Color { get; set; } = string.Empty;
}
