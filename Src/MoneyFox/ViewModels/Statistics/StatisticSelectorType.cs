namespace MoneyFox.ViewModels.Statistics
{
    using Core;
    using Core.Enums;

    public class StatisticSelectorType
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public StatisticType Type { get; set; }
    }
}