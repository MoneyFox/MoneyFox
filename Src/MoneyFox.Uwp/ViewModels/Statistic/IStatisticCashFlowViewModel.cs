using MoneyFox.Application.Statistics;
using System.Collections.ObjectModel;

namespace MoneyFox.Uwp.ViewModels.Statistic
{
    public interface IStatisticCashFlowViewModel
    {
        ObservableCollection<StatisticEntry> StatisticItems { get; }
    }
}