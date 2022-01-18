using CommunityToolkit.Mvvm.Input;
using MoneyFox.Core;
using MoneyFox.Core.Resources;
using MoneyFox.Uwp.ViewModels.Statistic;
using MoneyFox.Uwp.ViewModels.Statistics;
using System.Collections.Generic;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimeStatisticSelectorViewModel : IStatisticSelectorViewModel
    {
        public List<StatisticSelectorType> StatisticItems => new List<StatisticSelectorType>
        {
            new StatisticSelectorType
            {
                Name = Strings.CashflowLabel,
                Description = Strings.CashflowDescription,
                Type = StatisticType.Cashflow
            },
            new StatisticSelectorType
            {
                Name = Strings.CategorySpreadingLabel,
                Description = Strings.CategorieSpreadingDescription,
                Type = StatisticType.CategorySpreading
            },
            new StatisticSelectorType
            {
                Name = Strings.CategorySummaryLabel,
                Description = Strings.CategorySummaryDescription,
                Type = StatisticType.CategorySummary
            }
        };

        public RelayCommand<StatisticSelectorType> GoToStatisticCommand { get; } = null!;
    }
}