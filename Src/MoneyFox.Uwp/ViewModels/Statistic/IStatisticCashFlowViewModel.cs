using MoneyFox.Application.Statistics;
using MoneyFox.Ui.Shared.Commands;
using System.Collections.ObjectModel;

namespace MoneyFox.Uwp.ViewModels.Statistic
{
    public interface IStatisticCashFlowViewModel
    {
        string Title { get; }

        ObservableCollection<StatisticEntry> StatisticItems { get; }

        AsyncCommand LoadedCommand { get; }
    }
}
