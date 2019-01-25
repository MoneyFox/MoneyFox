using System.Collections.ObjectModel;
using Microcharts;
using MoneyFox.ServiceLayer;

namespace MoneyFox.Business.ViewModels.Statistic
{
    public interface IStatisticCashFlowViewModel
    {
        string Title { get; }
        BarChart Chart { get; }
        ObservableCollection<StatisticEntry> StatisticItems { get; }
    }
}