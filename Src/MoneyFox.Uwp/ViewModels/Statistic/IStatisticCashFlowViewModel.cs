using System.Collections.ObjectModel;
using Microcharts;
using MoneyFox.Application.Statistics;
using MoneyFox.Ui.Shared.Commands;

namespace MoneyFox.Uwp.ViewModels.Statistic
{
    public interface IStatisticCashFlowViewModel
    {
        string Title { get; }
        BarChart Chart { get; }
        ObservableCollection<StatisticEntry> StatisticItems { get; }

        AsyncCommand LoadedCommand { get; }
    }
}
