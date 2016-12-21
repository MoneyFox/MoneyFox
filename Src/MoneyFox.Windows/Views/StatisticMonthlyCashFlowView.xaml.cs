using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using MoneyFox.Windows.Views.Dialogs;

namespace MoneyFox.Windows.Views
{
    public sealed partial class StatisticMonthlyCashFlowView : IDisposable
    {
        public StatisticMonthlyCashFlowView()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            ExpensePlotView.Model = null;
        }

        private async void SetDate(object sender, RoutedEventArgs e)
        {
            await new SelectDateRangeDialog().ShowAsync();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ExpensePlotView.Model = null;
            ExpensePlotView = null;

            base.OnNavigatingFrom(e);
        }
    }
}