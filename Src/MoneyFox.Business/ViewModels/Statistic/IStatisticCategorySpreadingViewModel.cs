using Microcharts;

namespace MoneyFox.Business.ViewModels.Statistic
{
    public interface IStatisticCategorySpreadingViewModel
    {
        string Title { get; }
        DonutChart Chart { get; }
    }
}