using System.Collections.ObjectModel;
using MoneyFox.Business.ViewModels.Statistic;
using MoneyFox.Foundation.Models;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimeStatisticCategorySummaryViewModel : BaseViewModel, IStatisticCategorySummaryViewModel
    {
        public ObservableCollection<StatisticItem> CategorySummary => new ObservableCollection<StatisticItem>
        {
            new StatisticItem
            {
                Label = "Einkaufen",
                Value = 745
            },
            new StatisticItem
            {
                Label = "Beeeeer",
                Value = 666
            }
        };
    }
}
