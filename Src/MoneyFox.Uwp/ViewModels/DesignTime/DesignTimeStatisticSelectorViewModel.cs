using System.Collections.Generic;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Presentation.ViewModels.Statistic;
using MoneyFox.Uwp.ViewModels.Statistic;

namespace MoneyFox.Presentation.ViewModels.DesignTime
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

        public RelayCommand<StatisticSelectorType> GoToStatisticCommand { get; }
    }
}
