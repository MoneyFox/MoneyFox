using System;
using Windows.UI.Xaml;
using MoneyFox.Windows.Views.Dialogs;

namespace MoneyFox.Windows.Views
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
        }
    }
}