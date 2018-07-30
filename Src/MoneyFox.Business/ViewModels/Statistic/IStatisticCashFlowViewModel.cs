using Microcharts;

namespace MoneyFox.Business.ViewModels.Statistic
{
    public interface IStatisticCashFlowViewModel
    {
        string Title { get; }
        BarChart Chart { get; }
    }
}