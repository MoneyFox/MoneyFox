using MoneyFox.Core.Queries.Statistics;
using System.Collections.ObjectModel;

namespace MoneyFox.Win.ViewModels.Statistics
{
    public interface IStatisticCategorySpreadingViewModel
    {
        ObservableCollection<StatisticEntry> StatisticItems { get; }
    }
}