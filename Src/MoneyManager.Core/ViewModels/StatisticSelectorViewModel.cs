using System.Collections.Generic;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;
using MvvmCross.Core.ViewModels;

namespace MoneyManager.Core.ViewModels
{
    public class StatisticSelectorViewModel : BaseViewModel
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
        public MvxCommand<StatisticSelectorType> GoToStatisticCommand
            => new MvxCommand<StatisticSelectorType>(GoToStatistic);

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