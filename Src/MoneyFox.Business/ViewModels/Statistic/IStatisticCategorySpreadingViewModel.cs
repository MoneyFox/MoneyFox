using System.Collections.ObjectModel;
using Microcharts;
using MoneyFox.Business.StatisticDataProvider;

namespace MoneyFox.Business.ViewModels.Statistic
{
    public interface IStatisticCategorySpreadingViewModel : IBaseViewModel
    {
        string Title { get; }
        DonutChart Chart { get; }
        ObservableCollection<StatisticEntry> StatisticItems { get; }
    }
}