#nullable enable
using MoneyFox.Core.Resources;
using MoneyFox.Uwp.ViewModels.Statistic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace MoneyFox.Uwp.Views.Statistics
{
    public sealed partial class StatisticCategorySpreadingView
    {
        public override bool ShowHeader => false;

        public StatisticCategorySpreadingViewModel ViewModel => (StatisticCategorySpreadingViewModel)DataContext;

        public override string Header => Strings.CategorySpreadingTitle;

        public StatisticCategorySpreadingView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.StatisticCategorySpreadingVm;
        }

        private void OpenFilterFlyout(object sender, RoutedEventArgs e) =>
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
    }
}