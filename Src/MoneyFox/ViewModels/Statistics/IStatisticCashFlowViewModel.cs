using Microcharts;
using MoneyFox.Ui.Shared.Commands;

namespace MoneyFox.ViewModels.Statistics
{
    public interface IStatisticCashFlowViewModel
    {
        string Title { get; }

        BarChart Chart { get; }

        RelayCommand LoadedCommand { get; }
    }
}
