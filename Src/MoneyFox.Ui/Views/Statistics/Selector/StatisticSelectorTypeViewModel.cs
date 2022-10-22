namespace MoneyFox.Ui.Views.Statistics.Selector;

using MoneyFox.Core.Enums;

public class StatisticSelectorTypeViewModel
{
    public string IconGlyph { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public StatisticType Type { get; set; }
}
