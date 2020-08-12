using GalaSoft.MvvmLight.Command;
using MoneyFox.ViewModels.Statistics;
using System.Collections.ObjectModel;

namespace MoneyFox.Presentation.ViewModels.Statistic
{
    /// <summary>
    /// Representation of the category summary statistic view
    /// </summary>
    public interface IStatisticCategorySummaryViewModel
    {
        /// <summary>
        /// The statistic items to display.
        /// </summary>
        ObservableCollection<CategoryOverviewViewModel> CategorySummary { get; }

        /// <summary>
        /// Indicates if there are data to display.
        /// </summary>
        bool HasData { get; }

        RelayCommand LoadedCommand { get; }
    }
}
