using System.Collections.ObjectModel;
using MoneyFox.BusinessLogic.StatisticDataProvider;

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
        ObservableCollection<CategoryOverviewItem> CategorySummary { get; }

        /// <summary>
        ///     Indicates if there are data to display.
        /// </summary>
        bool HasData { get; }
    }
}