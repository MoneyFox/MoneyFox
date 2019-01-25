using System.Collections.ObjectModel;
using Microcharts;
using MoneyFox.ServiceLayer;

namespace MoneyFox.Business.ViewModels.Statistic
{
    public interface IStatisticCategorySpreadingViewModel : IBaseViewModel
    {
        string Title { get; }
        DonutChart Chart { get; }
        ObservableCollection<StatisticEntry> StatisticItems { get; }
    }
}