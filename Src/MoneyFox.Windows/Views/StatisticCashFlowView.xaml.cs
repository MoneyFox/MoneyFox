using System;
using Windows.UI.Xaml;
using MoneyFox.Windows.Views.Dialogs;

namespace MoneyFox.Windows.Views
{
    public sealed partial class StatisticCashFlowView
    {
        public StatisticCashFlowView()
        {
            InitializeComponent();
        }

        private async void SetDate(object sender, RoutedEventArgs e)
        {
            await new SelectDateRangeDialog().ShowAsync();
        }
    }
}