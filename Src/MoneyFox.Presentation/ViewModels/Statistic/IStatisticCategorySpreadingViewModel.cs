using System.Collections.ObjectModel;
using Microcharts;
using MoneyFox.Application.Statistics;
using MoneyFox.Presentation.Commands;

namespace MoneyFox.Presentation.ViewModels.Statistic
{
    public interface IStatisticCategorySpreadingViewModel
    {
        string Title { get; }
        DonutChart Chart { get; }
        ObservableCollection<StatisticEntry> StatisticItems { get; }

        AsyncCommand LoadedCommand { get; }
    }
}
