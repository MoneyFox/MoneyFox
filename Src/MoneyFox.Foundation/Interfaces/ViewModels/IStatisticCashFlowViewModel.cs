using MoneyFox.Foundation.Models;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Foundation.Interfaces.ViewModels
{
    public interface IStatisticCashFlowViewModel
    {
        string Title { get; }

        MvxObservableCollection<StatisticItem> StatisticItems { get; }
    }
}