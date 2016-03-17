using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Model;
using MoneyFox.Foundation.Resources;
using MoneyManager.Foundation;

namespace MoneyFox.Core.ViewModels
{
    public class StatisticSelectorViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;

        public StatisticSelectorViewModel(INavigationService navigationService)
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
                Name = Strings.SpreadingLabel,
                Description = Strings.CategorieSpreadingDescription,
                Type = StatisticType.CategorySpreading
            },
            new StatisticSelectorType
            {
                Name = Strings.CategorySummary,
                Description = Strings.CategorySummaryDescription,
                Type = StatisticType.CategorySummary
            },
            new StatisticSelectorType
            {
                Name = Strings.ExpenseHistory,
                Description = Strings.ExpenseHistoryDescription,
                Type = StatisticType.ExpenseHistory
            }
        };

        /// <summary>
        ///     Navigates to the statistic view and shows the selected statistic
        /// </summary>
        public RelayCommand<StatisticSelectorType> GoToStatisticCommand
            => new RelayCommand<StatisticSelectorType>(GoToStatistic);

        private void GoToStatistic(StatisticSelectorType item)
        {
            switch (item.Type)
            {
                case StatisticType.Cashflow:
                    navigationService.NavigateTo(NavigationConstants.STATISTIC_CASH_FLOW_VIEW);
                    break;

                case StatisticType.CategorySpreading:
                    navigationService.NavigateTo(NavigationConstants.STATISTIC_CATEGORY_SPREADING_VIEW);
                    break;

                case StatisticType.CategorySummary:
                    navigationService.NavigateTo(NavigationConstants.STATISTIC_CATEGORY_SUMMARY_VIEW);
                    break;

                case StatisticType.ExpenseHistory:
                    navigationService.NavigateTo(NavigationConstants.STATISTIC_MONTHLY_EXPENSES_VIEW);
                    break;
            }
        }
    }
}