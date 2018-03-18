using Microcharts;

namespace MoneyFox.Business.ViewModels.Interfaces
{
    public interface IStatisticCategorySpreadingViewModel
    {
        string Title { get; }
        DonutChart Chart { get; }
    }
}