using System.Collections.ObjectModel;
using Microcharts;
using MoneyFox.BusinessLogic.StatisticDataProvider;

namespace MoneyFox.ServiceLayer.ViewModels.Statistic
{
    public interface IStatisticCategorySpreadingViewModel : IBaseViewModel
    {
        string Title { get; }
        DonutChart Chart { get; }
        ObservableCollection<StatisticEntry> StatisticItems { get; }
    }
}