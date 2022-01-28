using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using MoneyFox.Core.Resources;

#nullable enable
namespace MoneyFox.Win.Pages.Statistics
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