using MoneyFox.Core.Resources;
using MoneyFox.Uwp.ViewModels.Statistic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

#nullable enable
namespace MoneyFox.Uwp.Views.Statistics
{
    public sealed partial class StatisticCategoryProgressionView
    {
        public StatisticCategoryProgressionViewModel ViewModel => (StatisticCategoryProgressionViewModel)DataContext;

        public override string Header => Strings.MonthlyCashflowTitle;

        public StatisticCategoryProgressionView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.StatisticCategoryProgressionVm;
        }

        private void OpenFilterFlyout(object sender, RoutedEventArgs e) =>
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
    }
}