using MoneyFox.Foundation.Models;
using MvvmCross.ViewModels;

namespace MoneyFox.ServiceLayer.ViewModels.Statistic
{
    /// <summary>
    ///     Representation of the category summary statistic view
    /// </summary>
    public interface IStatisticCategorySummaryViewModel
    {
        /// <summary>
        ///     The statistic items to display.
        /// </summary>
        MvxObservableCollection<StatisticItem> CategorySummary { get; }

        /// <summary>
        ///     Indicates if there are data to display.
        /// </summary>
        bool HasData { get; }
    }
}