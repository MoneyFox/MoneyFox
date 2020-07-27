using Microcharts;
using MoneyFox.Ui.Shared.Commands;

namespace MoneyFox.Presentation.ViewModels.Statistic
{
    public interface IStatisticCategorySpreadingViewModel
    {
        string Title { get; }

        DonutChart Chart { get; }

        AsyncCommand LoadedCommand { get; }
    }
}
