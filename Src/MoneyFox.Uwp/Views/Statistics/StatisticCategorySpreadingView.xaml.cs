#nullable enable
using MoneyFox.Application.Resources;
using MoneyFox.Uwp.ViewModels.Statistics;

namespace MoneyFox.Uwp.Views.Statistics
{
    public sealed partial class StatisticCategorySpreadingView
    {
        public override string Header => Strings.CategorySpreadingTitle;

        public StatisticCategorySpreadingViewModel ViewModel => (StatisticCategorySpreadingViewModel)DataContext;

        public StatisticCategorySpreadingView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.StatisticCategorySpreadingVm;
        }
    }
}