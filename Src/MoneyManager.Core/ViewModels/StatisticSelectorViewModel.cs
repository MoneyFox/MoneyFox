using System;
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
            new StatisticSelectorType {Name = Strings.CashflowLabel},
            new StatisticSelectorType {Name = Strings.SpreadingLabel},
            new StatisticSelectorType {Name = Strings.CategorySummary},
        };

        /// <summary>
        ///     Navigates to the statistic view and shows the selected statistic
        /// </summary>
        public MvxCommand<StatisticSelectorType> GoToStatisticCommand => new MvxCommand<StatisticSelectorType>(GoToStatistic);

        private void GoToStatistic(StatisticSelectorType item)
        {
            switch(item.Type)
            {
                case StatisticType.Cashflow:
                    ShowViewModel<CashFlowViewModel>();
                    break;

                case StatisticType.CategorySpreading:
                    ShowViewModel<SpreadingViewModel>();
                    break;

                case StatisticType.CategorySummary:
                    ShowViewModel<CategorySummaryViewModel>();
                    break;
            }
        }
    }
}
