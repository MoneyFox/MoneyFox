﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.ViewModels.Statistic.StatisticCategorySummary;
using MoneyFox.Uwp.ViewModels.Statistics;
using System.Collections.Generic;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Statistic
{
    public class StatisticSelectorViewModel : ObservableObject, IStatisticSelectorViewModel
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
                Name = Strings.MonthlyCashflowLabel,
                Description = Strings.MonthlyCashflowDescription,
                Type = StatisticType.MonthlyAccountCashFlow
            },
            new StatisticSelectorType
            {
                Name = Strings.CategoryProgressionLabel,
                Description = Strings.CategoryProgressionDescription,
                Type = StatisticType.CategoryProgression
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
        public RelayCommand<StatisticSelectorType> GoToStatisticCommand =>
            new RelayCommand<StatisticSelectorType>(GoToStatistic);

        private void GoToStatistic(StatisticSelectorType item)
        {
            if(item.Type == StatisticType.Cashflow)
            {
                navigationService.Navigate<StatisticCashFlowViewModel>();
            }
            else if(item.Type == StatisticType.MonthlyAccountCashFlow)
            {
                navigationService.Navigate<StatisticAccountMonthlyCashflowViewModel>();
            }
            else if(item.Type == StatisticType.CategorySpreading)
            {
                navigationService.Navigate<StatisticCategorySpreadingViewModel>();
            }
            else if(item.Type == StatisticType.CategorySummary)
            {
                navigationService.Navigate<StatisticCategorySummaryViewModel>();
            }
            else if(item.Type == StatisticType.CategoryProgression)
            {
                navigationService.Navigate<StatisticCategoryProgressionViewModel>();
            }
        }
    }
}