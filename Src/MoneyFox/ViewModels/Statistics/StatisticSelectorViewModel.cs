namespace MoneyFox.ViewModels.Statistics
{

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using Core.Enums;
    using Core.Resources;

    public class StatisticSelectorViewModel : ObservableObject, IStatisticSelectorViewModel
    {
        /// <summary>
        ///     All possible statistic to choose from
        /// </summary>
        public List<StatisticSelectorTypeViewModel> StatisticItems
            => new List<StatisticSelectorTypeViewModel>
            {
                new StatisticSelectorTypeViewModel
                {
                    IconGlyph = "bar_chart",
                    Name = Strings.CashflowLabel,
                    Description = Strings.CashflowDescription,
                    Type = StatisticType.Cashflow
                },
                new StatisticSelectorTypeViewModel
                {
                    IconGlyph = "bar_chart",
                    Name = Strings.MonthlyCashflowLabel,
                    Description = Strings.MonthlyCashflowDescription,
                    Type = StatisticType.MonthlyAccountCashFlow
                },
                new StatisticSelectorTypeViewModel
                {
                    IconGlyph = "bar_chart",
                    Name = Strings.CategoryProgressionLabel,
                    Description = Strings.CategoryProgressionDescription,
                    Type = StatisticType.CategoryProgression
                },
                new StatisticSelectorTypeViewModel
                {
                    IconGlyph = "donut_large",
                    Name = Strings.CategorySpreadingLabel,
                    Description = Strings.CategorieSpreadingDescription,
                    Type = StatisticType.CategorySpreading
                },
                new StatisticSelectorTypeViewModel
                {
                    IconGlyph = "format_list_bulleted",
                    Name = Strings.CategorySummaryLabel,
                    Description = Strings.CategorySummaryDescription,
                    Type = StatisticType.CategorySummary
                }
            };

        /// <summary>
        ///     Navigates to the statistic view and shows the selected statistic
        /// </summary>
        public RelayCommand<StatisticSelectorTypeViewModel> GoToStatisticCommand
            => new RelayCommand<StatisticSelectorTypeViewModel>(async s => await GoToStatisticAsync(s));

        private static async Task GoToStatisticAsync(StatisticSelectorTypeViewModel item)
        {
            if (item.Type == StatisticType.Cashflow)
            {
                await Shell.Current.GoToAsync(ViewModelLocator.StatisticCashFlowRoute);
            }
            else if (item.Type == StatisticType.CategorySpreading)
            {
                await Shell.Current.GoToAsync(ViewModelLocator.StatisticCategorySpreadingRoute);
            }
            else if (item.Type == StatisticType.CategorySummary)
            {
                await Shell.Current.GoToAsync(ViewModelLocator.StatisticCategorySummaryRoute);
            }
            else if (item.Type == StatisticType.MonthlyAccountCashFlow)
            {
                await Shell.Current.GoToAsync(ViewModelLocator.StatisticAccountMonthlyCashFlowRoute);
            }
            else if (item.Type == StatisticType.CategoryProgression)
            {
                await Shell.Current.GoToAsync(ViewModelLocator.StatisticCategoryProgressionRoute);
            }
        }
    }

}
