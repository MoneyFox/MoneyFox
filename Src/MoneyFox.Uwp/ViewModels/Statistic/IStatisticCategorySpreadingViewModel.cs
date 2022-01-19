using MoneyFox.Core.Queries.Statistics;
using System.Collections.ObjectModel;

namespace MoneyFox.Uwp.ViewModels.Statistic
{
    public interface IStatisticCategorySpreadingViewModel
    {
        ObservableCollection<StatisticEntry> StatisticItems { get; }
    }
}