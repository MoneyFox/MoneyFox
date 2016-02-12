using System;
using Windows.UI.Xaml;
using MoneyManager.Windows.Dialogs;

namespace MoneyManager.Windows.Views
{
    public sealed partial class StatisticCategorySpreadingPage : IDisposable
    {
        public StatisticCategorySpreadingPage()
        {
            InitializeComponent();
        }

        private async void SetDate(object sender, RoutedEventArgs e)
        {
            await new SelectDateRangeDialog().ShowAsync();
        }

        public void Dispose()
        {
            SpreadingPlotView.Model = null;
            SpreadingPlotView = null;
        }
    }
}