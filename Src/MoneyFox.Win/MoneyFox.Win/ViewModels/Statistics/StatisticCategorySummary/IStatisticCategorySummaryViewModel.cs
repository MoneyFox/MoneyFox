namespace MoneyFox.Win.ViewModels.Statistics.StatisticCategorySummary;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

internal interface IStatisticCategorySummaryViewModel
{
    ObservableCollection<CategoryOverviewViewModel> CategorySummary { get; }
    bool HasData { get; }
    IncomeExpenseBalanceViewModel IncomeExpenseBalance { get; set; }
    RelayCommand<CategoryOverviewViewModel> SummaryEntrySelectedCommand { get; }
}
