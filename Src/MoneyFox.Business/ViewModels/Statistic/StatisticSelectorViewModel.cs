using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Models;
using MoneyFox.Foundation.Resources;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.Statistic
{
    public class StatisticSelectorViewModel : MvxViewModel, IStatisticSelectorViewModel
    {
        private readonly IMvxNavigationService navigationService;

        /// <summary>
        ///     Constructor
        /// </summary>
        public StatisticSelectorViewModel(IMvxNavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        /// <summary>
        ///     All possible statistic to choose from
        /// </summary>
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

        /// <summary>
        ///     Navigates to the statistic view and shows the selected statistic
        /// </summary>
        public MvxAsyncCommand<StatisticSelectorType> GoToStatisticCommand
            => new MvxAsyncCommand<StatisticSelectorType>(GoToStatistic);

        private async Task GoToStatistic(StatisticSelectorType item)
        {
            switch (item.Type)
            {
                case StatisticType.Cashflow:
                    await navigationService.Navigate<StatisticCashFlowViewModel>();
                    break;

                case StatisticType.CategorySpreading:
                    await navigationService.Navigate<StatisticCategorySpreadingViewModel>();
                    break;

                case StatisticType.CategorySummary:
                    await navigationService.Navigate<StatisticCategorySummaryViewModel>();
                    break;
            }
        }
    }
}