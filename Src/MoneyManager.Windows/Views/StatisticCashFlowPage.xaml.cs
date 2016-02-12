using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using MoneyManager.Core.ViewModels;
using MoneyManager.Windows.Dialogs;

namespace MoneyManager.Windows.Views
{
    public sealed partial class StatisticCashFlowPage : IDisposable
    {
        public StatisticCashFlowPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            CashFlowPlotView.Model = (DataContext as StatisticCashFlowViewModel)?.CashFlowModel;
        }

        private async void SetDate(object sender, RoutedEventArgs e)
        {
            await new SelectDateRangeDialog().ShowAsync();
            (DataContext as StatisticCashFlowViewModel)?.LoadCommand.Execute();
        }

        public void Dispose()
        {
            CashFlowPlotView.Model = null;
            //CashFlowPlotView = null;
        }
    }
}
