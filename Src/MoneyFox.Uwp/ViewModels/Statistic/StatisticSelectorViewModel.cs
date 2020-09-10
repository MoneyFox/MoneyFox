using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.ViewModels.Statistic;
using MoneyFox.Uwp.ViewModels.Statistic.StatisticCategorySummary;

namespace MoneyFox.Presentation.ViewModels.Statistic
{
    public class StatisticSelectorViewModel : ViewModelBase, IStatisticSelectorViewModel
    {
        private readonly INavigationService navigationService;

        /// <summary>
        ///     Constructor
        /// </summary>
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
        public RelayCommand<StatisticSelectorType> GoToStatisticCommand => new RelayCommand<StatisticSelectorType>(GoToStatistic);

        private void GoToStatistic(StatisticSelectorType item)
        {
            if(item.Type == StatisticType.Cashflow)
            {
                navigationService.Navigate<StatisticCashFlowViewModel>();
            }
            else if(item.Type == StatisticType.CategorySpreading)
            {
                navigationService.Navigate<StatisticCategorySpreadingViewModel>();
            }
            else if(item.Type == StatisticType.CategorySummary)
            {
                navigationService.Navigate<StatisticCategorySummaryViewModel>();
            }
        }
    }
}
