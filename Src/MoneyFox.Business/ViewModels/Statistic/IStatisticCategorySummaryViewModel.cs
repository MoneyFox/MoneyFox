using System.Collections.ObjectModel;
using MoneyFox.Foundation.Models;

namespace MoneyFox.Business.ViewModels.Statistic
{
    /// <summary>
    ///     Representation of the category summary statistic view
    /// </summary>
    public interface IStatisticCategorySummaryViewModel
    {
        /// <summary>
        ///     the statistic items to display.
        /// </summary>
        ObservableCollection<StatisticItem> CategorySummary { get; }
    }
}