using MoneyFox.Application.Resources;
using MoneyFox.Uwp.ViewModels.Statistic;

#nullable enable
namespace MoneyFox.Uwp.Views.Statistics
{
    public sealed partial class StatisticCategorySpreadingView
    {
        public StatisticCategorySpreadingViewModel ViewModel => (StatisticCategorySpreadingViewModel)DataContext;

        public override string Header => Strings.CategorySpreadingTitle;

        public StatisticCategorySpreadingView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.StatisticCategorySpreadingVm;
        }
    }
}
