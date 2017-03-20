using System.Collections.ObjectModel;
using MoneyFox.Foundation.Models;

namespace MoneyFox.Foundation.Interfaces.ViewModels
{
    public interface IStatisticCategorySummaryViewModel
    {
        ObservableCollection<StatisticItem> CategorySummary { get; }
    }
}