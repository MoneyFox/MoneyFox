using MoneyFox.Foundation.Models;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.Interfaces
{
    public interface IStatisticCategorySpreadingViewModel
    {
        string Title { get; }
        MvxObservableCollection<StatisticItem> StatisticItems { get; }
    }
}