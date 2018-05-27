using MoneyFox.Business.ViewModels.Statistic;
using MoneyFox.Foundation.Models;
using MvvmCross.ViewModels;

namespace MoneyFox.Windows.DesignTime
{
    public class DesignTimeStatisticCategorySummaryViewModel: IStatisticCategorySummaryViewModel
    {
        public MvxObservableCollection<StatisticItem> CategorySummary => new MvxObservableCollection<StatisticItem>
            {
                new StatisticItem{ Value = 300, Label = "Foo"},
                new StatisticItem{ Value = 123, Label = "beer"},
                new StatisticItem{ Value = 234, Label = "beer again"},
                new StatisticItem{ Value = 3122, Label = "Other Foo"},
                new StatisticItem{ Value = 300, Label = "Lorem Ipsum"},
                new StatisticItem{ Value = 332, Label = "Foood"},
                new StatisticItem{ Value = 251, Label = "Clothes"},
                new StatisticItem{ Value = 38987987, Label = "Rent"},
                new StatisticItem{ Value = 84654, Label = "A lot of Fooo"},
                new StatisticItem{ Value = 2135, Label = "Something else"}
            };

        public bool HasData { get; }
    }
}
