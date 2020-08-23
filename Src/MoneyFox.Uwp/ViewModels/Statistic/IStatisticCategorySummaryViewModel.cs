using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;

namespace MoneyFox.Uwp.ViewModels.Statistic
{
    public interface IStatisticCategorySummaryViewModel
    {
        ObservableCollection<CategoryOverviewViewModel> CategorySummary { get; }
        bool HasData { get; }
        IncomeExpenseBalanceViewModel IncomeExpenseBalance { get; set; }
        RelayCommand<CategoryOverviewViewModel> SummaryEntrySelectedCommand { get; }
    }
}