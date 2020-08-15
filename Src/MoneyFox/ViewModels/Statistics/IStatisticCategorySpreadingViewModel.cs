using GalaSoft.MvvmLight.Command;
using Microcharts;
using MoneyFox.Ui.Shared.Commands;

namespace MoneyFox.Presentation.ViewModels.Statistic
{
    public interface IStatisticCategorySpreadingViewModel
    {
        string Title { get; }

        DonutChart Chart { get; }

        RelayCommand LoadedCommand { get; }
    }
}
