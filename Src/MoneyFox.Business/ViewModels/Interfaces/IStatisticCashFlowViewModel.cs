using Microcharts;

namespace MoneyFox.Business.ViewModels.Interfaces
{
    public interface IStatisticCashFlowViewModel
    {
        string Title { get; }
        BarChart Chart { get; }
    }
}