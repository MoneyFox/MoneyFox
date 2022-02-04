using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using MoneyFox.Core.Resources;
using MoneyFox.Win.ViewModels.Statistics;

namespace MoneyFox.Win.Pages.Statistics
{
    public sealed partial class StatisticCategoryProgressionPage
    {
        public StatisticCategoryProgressionViewModel ViewModel => (StatisticCategoryProgressionViewModel)DataContext;

        public override string Header => Strings.MonthlyCashflowTitle;

        public StatisticCategoryProgressionPage()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.StatisticCategoryProgressionVm;
        }

        private void OpenFilterFlyout(object sender, RoutedEventArgs e) =>
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
    }
}