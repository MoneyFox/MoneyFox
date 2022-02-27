namespace MoneyFox.Win.ViewModels.Statistics.StatisticCategorySummary;

using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;
using System.Collections.ObjectModel;

[UsedImplicitly]
public class DesignTimeStatisticCategorySummaryViewModel : IStatisticCategorySummaryViewModel
{
    public RelayCommand LoadedCommand { get; } = null!;

    /// <inheritdoc />
    public IncomeExpenseBalanceViewModel IncomeExpenseBalance { get; set; }
        = new() {TotalEarned = 400, TotalSpent = 600};

    public ObservableCollection<CategoryOverviewViewModel> CategorySummary
        => new()
        {
            new() {Label = "Einkaufen", Value = 745, Percentage = 30},
            new() {Label = "Beeeeer", Value = 666, Percentage = 70}
        };

    public bool HasData { get; } = true;
    public RelayCommand<CategoryOverviewViewModel> SummaryEntrySelectedCommand { get; } = null!;
}