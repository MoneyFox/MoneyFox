using System.Collections.ObjectModel;
using MoneyFox.Foundation.Models;

namespace MoneyFox.Business.ViewModels.Interfaces
{
    public interface IStatisticCategorySummaryViewModel
    {
        ObservableCollection<StatisticItem> CategorySummary { get; }
    }
}