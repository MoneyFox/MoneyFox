namespace MoneyFox.Ui.Views.Statistics.Selector;

using CommunityToolkit.Mvvm.Input;
using Core.Enums;
using Resources.Strings;
using ViewModels;

internal sealed class StatisticSelectorViewModel : BaseViewModel, IStatisticSelectorViewModel
{
    public List<StatisticSelectorTypeViewModel> StatisticItems
        => new()
        {
            new()
            {
                IconGlyph = IconFont.ChartBar,
                Name = Translations.CashflowLabel,
                Description = Translations.CashflowDescription,
                Type = StatisticType.Cashflow
            },
            new()
            {
                IconGlyph = IconFont.ChartBar,
                Name = Translations.MonthlyCashflowLabel,
                Description = Translations.MonthlyCashflowDescription,
                Type = StatisticType.MonthlyAccountCashFlow
            },
            new()
            {
                IconGlyph = IconFont.ChartBar,
                Name = Translations.CategoryProgressionLabel,
                Description = Translations.CategoryProgressionDescription,
                Type = StatisticType.CategoryProgression
            },
            new()
            {
                IconGlyph = IconFont.ChartDonut,
                Name = Translations.CategorySpreadingLabel,
                Description = Translations.CategorieSpreadingDescription,
                Type = StatisticType.CategorySpreading
            },
            new()
            {
                IconGlyph = IconFont.FormatListBulleted,
                Name = Translations.CategorySummaryLabel,
                Description = Translations.CategorySummaryDescription,
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
