namespace MoneyFox.Win.ViewModels.Statistics;

using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using Core.Enums;
using Core.Resources;
using Services;
using StatisticCategorySummary;

internal class StatisticSelectorViewModel : BaseViewModel, IStatisticSelectorViewModel
{
    private readonly INavigationService navigationService;

    public StatisticSelectorViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;
    }

    public List<StatisticSelectorType> StatisticItems
        => new()
        {
            new() { Name = Strings.CashflowLabel, Description = Strings.CashflowDescription, Type = StatisticType.Cashflow },
            new() { Name = Strings.MonthlyCashflowLabel, Description = Strings.MonthlyCashflowDescription, Type = StatisticType.MonthlyAccountCashFlow },
            new()
            {
                Name = Strings.CategoryProgressionLabel, Description = Strings.CategoryProgressionDescription, Type = StatisticType.CategoryProgression
            },
            new() { Name = Strings.CategorySpreadingLabel, Description = Strings.CategorieSpreadingDescription, Type = StatisticType.CategorySpreading },
            new() { Name = Strings.CategorySummaryLabel, Description = Strings.CategorySummaryDescription, Type = StatisticType.CategorySummary }
        };

    /// <summary>
    ///     Navigates to the statistic view and shows the selected statistic
    /// </summary>
    public RelayCommand<StatisticSelectorType> GoToStatisticCommand => new(GoToStatistic);

    private void GoToStatistic(StatisticSelectorType item)
    {
        if (item.Type == StatisticType.Cashflow)
        {
            navigationService.Navigate<StatisticCashFlowViewModel>();
        }
        else if (item.Type == StatisticType.MonthlyAccountCashFlow)
        {
            navigationService.Navigate<StatisticAccountMonthlyCashflowViewModel>();
        }
        else if (item.Type == StatisticType.CategorySpreading)
        {
            navigationService.Navigate<StatisticCategorySpreadingViewModel>();
        }
        else if (item.Type == StatisticType.CategorySummary)
        {
            navigationService.Navigate<StatisticCategorySummaryViewModel>();
        }
        else if (item.Type == StatisticType.CategoryProgression)
        {
            navigationService.Navigate<StatisticCategoryProgressionViewModel>();
        }
    }
}
