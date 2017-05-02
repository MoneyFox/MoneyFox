using MoneyFox.Foundation.Models;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.Interfaces
{
    public interface IStatisticCashFlowViewModel
    {
        string Title { get; }

        MvxObservableCollection<StatisticItem> StatisticItems { get; }
    }
}