namespace MoneyFox.Ui.Views.Statistics.Selector;

using CashFlow;
using CategoryProgression;
using CategorySpreading;
using CategorySummary;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using MonthlyAccountCashFlow;
using Resources.Strings;

internal sealed class StatisticSelectorViewModel(INavigationService navigationService) : NavigableViewModel
{
    public static List<StatisticSelectorTypeViewModel> StatisticItems
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

    public AsyncRelayCommand<StatisticSelectorTypeViewModel> GoToStatisticCommand => new(async s => await GoToStatisticAsync(s!.Type));

    private Task GoToStatisticAsync(StatisticType type)
    {
        return type switch
        {
            StatisticType.Cashflow => navigationService.GoTo<StatisticCashFlowViewModel>(),
            StatisticType.CategorySpreading => navigationService.GoTo<StatisticCategorySpreadingViewModel>(),
            StatisticType.CategorySummary => navigationService.GoTo<StatisticCategorySummaryViewModel>(),
            StatisticType.MonthlyAccountCashFlow => navigationService.GoTo<StatisticAccountMonthlyCashFlowViewModel>(),
            StatisticType.CategoryProgression => navigationService.GoTo<StatisticCategoryProgressionViewModel>(),
            StatisticType.CashflowHistory => Task.CompletedTask,
            _ => Task.CompletedTask
        };
    }
}
