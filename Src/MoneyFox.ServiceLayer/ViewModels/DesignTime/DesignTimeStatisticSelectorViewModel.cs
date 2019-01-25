using System.Collections.Generic;
using System.Globalization;
using MoneyFox.Business.ViewModels.Statistic;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Models;
using MoneyFox.Foundation.Resources;
using MvvmCross.Commands;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimeStatisticSelectorViewModel : IStatisticSelectorViewModel
    {
        public LocalizedResources Resources { get; } = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);

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

        public MvxAsyncCommand<StatisticSelectorType> GoToStatisticCommand { get; }
    }
}
