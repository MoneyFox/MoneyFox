using MoneyFox.Business.ViewModels.Statistic;
using MoneyFox.Foundation.Models;
using MvvmCross.ViewModels;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimeStatisticCategorySummaryViewModel : BaseViewModel, IStatisticCategorySummaryViewModel
    {
        public MvxObservableCollection<StatisticItem> CategorySummary => new MvxObservableCollection<StatisticItem>
        {
            new StatisticItem
            {
                Label = "Einkaufen",
                Value = 745,
                Percentage = 30
            },
            new StatisticItem
            {
                Label = "Beeeeer",
                Value = 666,
                Percentage = 70
            }
        };

        public bool HasData { get; } = true;
    }
}
