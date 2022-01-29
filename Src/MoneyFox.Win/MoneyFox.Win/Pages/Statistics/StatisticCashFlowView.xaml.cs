using MoneyFox.Core.Resources;

#nullable enable
namespace MoneyFox.Win.Pages.Statistics
{
    public sealed partial class StatisticCashFlowView
    {
        public StatisticCashFlowViewModel ViewModel => (StatisticCashFlowViewModel)DataContext;

        public override string Header => Strings.CashFlowStatisticTitle;

        public StatisticCashFlowView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.StatisticCashFlowVm;
        }

        private void OpenFilterFlyout(object sender, RoutedEventArgs e) =>
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
    }
}