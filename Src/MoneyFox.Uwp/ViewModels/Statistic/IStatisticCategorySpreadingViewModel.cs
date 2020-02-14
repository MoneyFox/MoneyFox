using System.Collections.ObjectModel;
using MoneyFox.Application.Statistics;
using MoneyFox.Ui.Shared.Commands;

namespace MoneyFox.Uwp.ViewModels.Statistic
{
    public interface IStatisticCategorySpreadingViewModel
    {
        string Title { get; }
        ObservableCollection<StatisticEntry> StatisticItems { get; }

        AsyncCommand LoadedCommand { get; }
    }
}
