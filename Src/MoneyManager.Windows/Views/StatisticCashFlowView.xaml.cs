using System;
using Windows.UI.Xaml;
using MoneyManager.Windows.Views.Dialogs;

namespace MoneyManager.Windows.Views
{
    public sealed partial class StatisticCashFlowView : IDisposable
    {
        public StatisticCashFlowView()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            CashFlowPlotView.Model = null;
        }

        private async void SetDate(object sender, RoutedEventArgs e)
        {
            await new SelectDateRangeDialog().ShowAsync();
            (DataContext as StatisticCashFlowViewModel)?.LoadCommand.Execute();
        }
    }
}