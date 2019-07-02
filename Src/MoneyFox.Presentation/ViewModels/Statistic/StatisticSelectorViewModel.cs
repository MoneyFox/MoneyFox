using System.Collections.Generic;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MoneyFox.Domain;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Models;
using MoneyFox.Foundation.Resources;

namespace MoneyFox.Presentation.ViewModels.Statistic
{
    public class StatisticSelectorViewModel : BaseViewModel, IStatisticSelectorViewModel
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
            switch (item.Type)
            {
                case StatisticType.Cashflow:
                    navigationService.NavigateTo(ViewModelLocator.StatisticCashFlow);
                    break;

                case StatisticType.CategorySpreading:
                    navigationService.NavigateTo(ViewModelLocator.StatisticCategorySpreading);
                    break;

                case StatisticType.CategorySummary:
                    navigationService.NavigateTo(ViewModelLocator.StatisticCategorySummary);
                    break;
            }
        }
    }
}