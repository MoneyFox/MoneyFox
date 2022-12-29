namespace MoneyFox.Ui.Views.Statistics.Selector;

using CommunityToolkit.Mvvm.Input;
using Core.Enums;
using Core.Resources;
using ViewModels;
using ViewModels.Statistics;

internal sealed class StatisticSelectorViewModel : BaseViewModel, IStatisticSelectorViewModel
{
    public List<StatisticSelectorTypeViewModel> StatisticItems
        => new()
        {
            new()
            {
                IconGlyph = IconFont.ChartBar,
                Name = Strings.CashflowLabel,
                Description = Strings.CashflowDescription,
                Type = StatisticType.Cashflow
            },
            new()
            {
                IconGlyph = IconFont.ChartBar,
                Name = Strings.MonthlyCashflowLabel,
                Description = Strings.MonthlyCashflowDescription,
                Type = StatisticType.MonthlyAccountCashFlow
            },
            new()
            {
                IconGlyph = IconFont.ChartBar,
                Name = Strings.CategoryProgressionLabel,
                Description = Strings.CategoryProgressionDescription,
                Type = StatisticType.CategoryProgression
            },
            new()
            {
                IconGlyph = IconFont.ChartDonut,
                Name = Strings.CategorySpreadingLabel,
                Description = Strings.CategorieSpreadingDescription,
                Type = StatisticType.CategorySpreading
            },
            new()
            {
                IconGlyph = IconFont.FormatListBulleted,
                Name = Strings.CategorySummaryLabel,
                Description = Strings.CategorySummaryDescription,
                Type = StatisticType.CategorySummary
            }
        };

    /// <summary>
    ///     Navigates to the statistic view and shows the selected statistic
    /// </summary>
    public RelayCommand<StatisticSelectorTypeViewModel> GoToStatisticCommand => new(async s => await GoToStatisticAsync(s));

    private static async Task GoToStatisticAsync(StatisticSelectorTypeViewModel item)
    {
        if (item.Type == StatisticType.Cashflow)
        {
            await Shell.Current.GoToAsync(Routes.StatisticCashFlowRoute);
        }
        else if (item.Type == StatisticType.CategorySpreading)
        {
            await Shell.Current.GoToAsync(Routes.StatisticCategorySpreadingRoute);
        }
        else if (item.Type == StatisticType.CategorySummary)
        {
            await Shell.Current.GoToAsync(Routes.StatisticCategorySummaryRoute);
        }
        else if (item.Type == StatisticType.MonthlyAccountCashFlow)
        {
            await Shell.Current.GoToAsync(Routes.StatisticAccountMonthlyCashFlowRoute);
        }
        else if (item.Type == StatisticType.CategoryProgression)
        {
            await Shell.Current.GoToAsync(Routes.StatisticCategoryProgressionRoute);
        }
    }
}
