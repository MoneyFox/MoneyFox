using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.ViewModels.Statistic;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    [SuppressMessage("Major Code Smell", "S109:Magic numbers should not be used", Justification = "Design Time")]
    public class DesignTimeStatisticCategorySummaryViewModel : IStatisticCategorySummaryViewModel
    {
        /// <inheritdoc />
        public IncomeExpenseBalanceViewModel IncomeExpenseBalance { get; } =
            new IncomeExpenseBalanceViewModel {TotalEarned = 400, TotalSpent = 600};

        public ObservableCollection<CategoryOverviewViewModel> CategorySummary => new ObservableCollection<CategoryOverviewViewModel>
        {
            new CategoryOverviewViewModel
            {
                Label = "Einkaufen",
                Value = 745,
                Percentage = 30
            },
            new CategoryOverviewViewModel
            {
                Label = "Beeeeer",
                Value = 666,
                Percentage = 70
            }
        };

        public bool HasData { get; } = true;
        public AsyncCommand LoadedCommand { get; }
    }
}
