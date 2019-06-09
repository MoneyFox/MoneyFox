using System.Collections.ObjectModel;
using Microcharts;
using MoneyFox.BusinessLogic.StatisticDataProvider;
using MoneyFox.ServiceLayer.ViewModels;

namespace MoneyFox.Presentation.ViewModels.Statistic
{
    public interface IStatisticCategorySpreadingViewModel : IBaseViewModel
    {
        string Title { get; }
        DonutChart Chart { get; }
        ObservableCollection<StatisticEntry> StatisticItems { get; }
    }
}