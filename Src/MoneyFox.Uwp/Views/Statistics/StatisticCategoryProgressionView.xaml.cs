using MoneyFox.Application.Resources;
using MoneyFox.Uwp.ViewModels.Statistics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

#nullable enable
namespace MoneyFox.Uwp.Views.Statistics
{
    public sealed partial class StatisticCategoryProgressionView
    {
        public StatisticCategoryProgressionViewModel ViewModel => (StatisticCategoryProgressionViewModel)DataContext;

        public override string Header => Strings.MonthlyCashflowTitle;

        public override bool ShowHeader => false;

        public StatisticCategoryProgressionView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.StatisticCategoryProgressionVm;
        }

        private void OpenFilterFlyout(object sender, RoutedEventArgs e) =>
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
    }
}