namespace MoneyFox.Win.ViewModels.Statistics.StatisticCategorySummary;

using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

public interface IStatisticCategorySummaryViewModel
{
    ObservableCollection<CategoryOverviewViewModel> CategorySummary { get; }
    bool HasData { get; }
    IncomeExpenseBalanceViewModel IncomeExpenseBalance { get; set; }
    RelayCommand<CategoryOverviewViewModel> SummaryEntrySelectedCommand { get; }
}