using System.Collections.ObjectModel;
using System.Globalization;
using MoneyFox.BusinessLogic.StatisticDataProvider;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Utilities;
using MoneyFox.ServiceLayer.ViewModels.Statistic;

namespace MoneyFox.ServiceLayer.ViewModels.DesignTime
{
    public class DesignTimeStatisticCategorySummaryViewModel : IStatisticCategorySummaryViewModel
    {
        public LocalizedResources Resources { get; } = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);

        public ObservableCollection<CategoryOverviewItem> CategorySummary => new ObservableCollection<CategoryOverviewItem>
        {
            new CategoryOverviewItem
            {
                Label = "Einkaufen",
                Value = 745,
                Percentage = 30
            },
            new CategoryOverviewItem
            {
                Label = "Beeeeer",
                Value = 666,
                Percentage = 70
            }
        };

        public bool HasData { get; } = true;
    }
}
