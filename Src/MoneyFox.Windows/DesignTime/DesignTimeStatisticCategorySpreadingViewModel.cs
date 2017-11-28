using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.Foundation.Models;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Windows.DesignTime
{
    public class DesignTimeStatisticCategorySpreadingViewModel : IStatisticCategorySpreadingViewModel
    {
        public string Title => "I AM A MIGHTY TITLE";

        public MvxObservableCollection<StatisticItem> StatisticItems => new MvxObservableCollection<StatisticItem>
        {
            new StatisticItem {Label = "Essen", Value = 1234},
            new StatisticItem {Label = "Bier", Value = 1465},
            new StatisticItem {Label = "Boooze", Value = 543},
            new StatisticItem {Label = "Rent", Value = 462},
            new StatisticItem {Label = "Clothes", Value = 1112},
            new StatisticItem {Label = "Eating or so?", Value = 512},
        };
    }
}
