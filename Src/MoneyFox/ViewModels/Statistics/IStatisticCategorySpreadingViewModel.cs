using GalaSoft.MvvmLight.Command;
using Microcharts;
using MoneyFox.Ui.Shared.Commands;

namespace MoneyFox.ViewModels.Statistics
{
    public interface IStatisticCategorySpreadingViewModel
    {
        string Title { get; }

        DonutChart Chart { get; }

        RelayCommand LoadedCommand { get; }
    }
}
