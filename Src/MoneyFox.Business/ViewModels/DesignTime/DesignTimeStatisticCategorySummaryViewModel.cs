using System.Collections.ObjectModel;
using MoneyFox.Foundation.Interfaces.ViewModels;
using MoneyFox.Foundation.Models;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimeStatisticCategorySummaryViewModel: IStatisticCategorySummaryViewModel
    {
        public ObservableCollection<StatisticItem> CategorySummary => new ObservableCollection<StatisticItem>
            {
                new StatisticItem{Category  = "Beeer", Value = 300, Label = "beer again"}
            };
    }
}
