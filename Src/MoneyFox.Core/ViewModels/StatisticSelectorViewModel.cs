using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Foundation.Resources;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;

namespace MoneyFox.Core.ViewModels
{
    public class StatisticSelectorViewModel : ViewModelBase
    {
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
                    ShowViewModel<StatisticCashFlowViewModel>();
                    break;

                case StatisticType.CategorySpreading:
                    ShowViewModel<StatisticCategorySpreadingViewModel>();
                    break;

                case StatisticType.CategorySummary:
                    ShowViewModel<StatisticCategorySummaryViewModel>();
                    break;

                case StatisticType.ExpenseHistory:
                    ShowViewModel<StatisticMonthlyExpensesViewModel>();
                    break;
            }
        }
    }
}