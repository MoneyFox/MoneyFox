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

    private async Task GoToStatisticAsync(StatisticType type)
    {
        switch (type)
        {
            case StatisticType.Cashflow:
                await navigationService.GoTo<StatisticCashFlowViewModel>();

                break;
            case StatisticType.CategorySpreading:
                await navigationService.GoTo<StatisticCategorySpreadingViewModel>();

                break;
            case StatisticType.CategorySummary:
                await navigationService.GoTo<StatisticCategorySummaryViewModel>();

                break;
            case StatisticType.MonthlyAccountCashFlow:
                await navigationService.GoTo<StatisticAccountMonthlyCashFlowViewModel>();

                break;
            case StatisticType.CategoryProgression:
                await navigationService.GoTo<StatisticCategoryProgressionViewModel>();

                break;
            case StatisticType.CashflowHistory:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type));
        }
    }
}
