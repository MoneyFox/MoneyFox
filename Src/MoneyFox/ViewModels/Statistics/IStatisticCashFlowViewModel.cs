using GalaSoft.MvvmLight.Command;
using Microcharts;

namespace MoneyFox.ViewModels.Statistics
{
    public interface IStatisticCashFlowViewModel
    {
        string Title { get; }

        BarChart Chart { get; }

        RelayCommand LoadedCommand { get; }
    }
}
