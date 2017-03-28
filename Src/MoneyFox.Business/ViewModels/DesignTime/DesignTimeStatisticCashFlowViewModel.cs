using MoneyFox.Foundation.Interfaces.ViewModels;
using MoneyFox.Foundation.Models;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimeStatisticCashFlowViewModel : IStatisticCashFlowViewModel
    {
        public string Title => "I AM A MIGHTY TITLE";

        public CashFlow CashFlow => new CashFlow
        {
            Expense = new StatisticItem { Label = "Expense", Category = "Expense",  Value = 1234},
            Income = new StatisticItem { Label = "Income", Category = "Income",  Value = 1465},
            Revenue = new StatisticItem { Label = "Revenue", Category = "Revenue",  Value = 543},
        };

        public MvxObservableCollection<StatisticItem> StatisticItems => new MvxObservableCollection<StatisticItem>
        {
            new StatisticItem {Label = "Expense", Value = 1234},
            new StatisticItem {Label = "Income", Value = 1465},
            new StatisticItem {Label = "Revenue", Value = 543},
        };

    }
}
