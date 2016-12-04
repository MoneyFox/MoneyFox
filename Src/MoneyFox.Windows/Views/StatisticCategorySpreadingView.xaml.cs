using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using MoneyFox.Windows.Views.Dialogs;

namespace MoneyFox.Windows.Views
{
    public sealed partial class StatisticCategorySpreadingView
    {
        public StatisticCategorySpreadingView()
        {
            InitializeComponent();
        }

        private async void SetDate(object sender, RoutedEventArgs e)
        {
            await new SelectDateRangeDialog().ShowAsync();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            SpreadingPlotView.Model = null;
            SpreadingPlotView = null;

            base.OnNavigatingFrom(e);
        }
    }
}