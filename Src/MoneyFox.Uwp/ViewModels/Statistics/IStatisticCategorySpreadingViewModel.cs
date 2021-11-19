using MoneyFox.Application.Statistics;
using System.Collections.ObjectModel;

namespace MoneyFox.Uwp.ViewModels.Statistics
{
    public interface IStatisticCategorySpreadingViewModel
    {
        ObservableCollection<StatisticEntry> StatisticItems { get; }
    }
}