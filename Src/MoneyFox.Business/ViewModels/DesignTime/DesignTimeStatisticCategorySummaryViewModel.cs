using MoneyFox.Business.ViewModels.Statistic;
using MoneyFox.Foundation.Models;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimeStatisticCategorySummaryViewModel : BaseViewModel, IStatisticCategorySummaryViewModel
    {
        public MvxObservableCollection<StatisticItem> CategorySummary => new MvxObservableCollection<StatisticItem>
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

        public bool HasData { get; }
    }
}
