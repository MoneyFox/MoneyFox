namespace MoneyFox.Ui.Views.Statistics.Selector;

using CommunityToolkit.Mvvm.Input;
using Resources.Strings;

internal sealed class StatisticSelectorViewModel : BasePageViewModel
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

    public AsyncRelayCommand<StatisticSelectorTypeViewModel> GoToStatisticCommand => new(async s => await GoToStatisticAsync(s!.Type));

    private static async Task GoToStatisticAsync(StatisticType type)
    {
        switch (type)
        {
            case StatisticType.Cashflow:
                await Shell.Current.GoToAsync(Routes.StatisticCashFlowRoute);

                break;
            case StatisticType.CategorySpreading:
                await Shell.Current.GoToAsync(Routes.StatisticCategorySpreadingRoute);

                break;
            case StatisticType.CategorySummary:
                await Shell.Current.GoToAsync(Routes.StatisticCategorySummaryRoute);

                break;
            case StatisticType.MonthlyAccountCashFlow:
                await Shell.Current.GoToAsync(Routes.StatisticAccountMonthlyCashFlowRoute);

                break;
            case StatisticType.CategoryProgression:
                await Shell.Current.GoToAsync(Routes.StatisticCategoryProgressionRoute);

                break;
            case StatisticType.CashflowHistory:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type));
        }
    }
}
