using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Models;
using MoneyFox.Foundation.Resources;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MoneyFox.ServiceLayer.ViewModels.Statistic
{
    public class StatisticSelectorViewModel : BaseNavigationViewModel, IStatisticSelectorViewModel
    {
        private readonly IMvxNavigationService navigationService;

        /// <summary>
        ///     Constructor
        /// </summary>
        public StatisticSelectorViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base (logProvider, navigationService)
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
                    await navigationService.Navigate<StatisticCashFlowViewModel>().ConfigureAwait(true);
                    break;

                case StatisticType.CategorySpreading:
                    await navigationService.Navigate<StatisticCategorySpreadingViewModel>().ConfigureAwait(true);
                    break;

                case StatisticType.CategorySummary:
                    await navigationService.Navigate<StatisticCategorySummaryViewModel>().ConfigureAwait(true);
                    break;
            }
        }
    }
}