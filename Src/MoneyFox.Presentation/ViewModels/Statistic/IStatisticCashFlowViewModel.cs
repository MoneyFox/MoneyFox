using Microcharts;
using MoneyFox.Ui.Shared.Commands;

namespace MoneyFox.Presentation.ViewModels.Statistic
{
    public interface IStatisticCashFlowViewModel
    {
        string Title { get; }
        BarChart Chart { get; }
        AsyncCommand LoadedCommand { get; }
    }
}
