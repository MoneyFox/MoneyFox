using CommunityToolkit.Mvvm.Input;
using MoneyFox.Core;
using MoneyFox.Core.Resources;
using System.Collections.Generic;

namespace MoneyFox.Win.ViewModels.DesignTime
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