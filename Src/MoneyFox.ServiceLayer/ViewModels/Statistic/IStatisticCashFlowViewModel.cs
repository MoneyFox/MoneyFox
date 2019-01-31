using System.Collections.ObjectModel;
using Microcharts;
using MoneyFox.BusinessLogic.StatisticDataProvider;

namespace MoneyFox.ServiceLayer.ViewModels.Statistic
{
    public interface IStatisticCashFlowViewModel
    {
        string Title { get; }
        BarChart Chart { get; }
        ObservableCollection<StatisticEntry> StatisticItems { get; }
    }
}