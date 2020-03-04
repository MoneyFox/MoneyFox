using System.Collections.ObjectModel;
using MoneyFox.Ui.Shared.Commands;

namespace MoneyFox.Uwp.ViewModels.Statistic
{
    /// <summary>
    ///     Representation of the category summary statistic view
    /// </summary>
    public interface IStatisticCategorySummaryViewModel
    {
        IncomeExpenseBalanceViewModel IncomeExpenseBalance { get; }

        /// <summary>
        ///     The statistic items to display.
        /// </summary>
        ObservableCollection<CategoryOverviewViewModel> CategorySummary { get; }

        /// <summary>
        ///     Indicates if there are data to display.
        /// </summary>
        bool HasData { get; }

        AsyncCommand LoadedCommand { get; }
    }
}
