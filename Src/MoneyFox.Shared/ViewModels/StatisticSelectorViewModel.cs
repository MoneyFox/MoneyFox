using System.Collections.Generic;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Shared.ViewModels {
    public class StatisticSelectorViewModel : BaseViewModel {
        /// <summary>
        ///     All possible statistic to choose from
        /// </summary>
        public List<StatisticSelectorType> StatisticItems => new List<StatisticSelectorType> {
            new StatisticSelectorType {
                Name = Strings.CashflowLabel,
                Description = Strings.CashflowDescription,
                Type = StatisticType.Cashflow
            },
            new StatisticSelectorType {
                Name = Strings.CategorySpreadingLabel,
                Description = Strings.CategorieSpreadingDescription,
                Type = StatisticType.CategorySpreading
            },
            new StatisticSelectorType {
                Name = Strings.CategorySummaryLabel,
                Description = Strings.CategorySummaryDescription,
                Type = StatisticType.CategorySummary
            },
            new StatisticSelectorType {
                Name = Strings.ExpenseHistoryLabel,
                Description = Strings.ExpenseHistoryDescription,
                Type = StatisticType.ExpenseHistory
            }
        };

        /// <summary>
        ///     Navigates to the statistic view and shows the selected statistic
        /// </summary>
        public MvxCommand<StatisticSelectorType> GoToStatisticCommand
            => new MvxCommand<StatisticSelectorType>(GoToStatistic);

        private void GoToStatistic(StatisticSelectorType item) {
            switch (item.Type) {
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