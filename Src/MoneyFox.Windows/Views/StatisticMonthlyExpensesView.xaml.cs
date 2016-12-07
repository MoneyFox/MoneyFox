using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using MoneyFox.Windows.Views.Dialogs;

namespace MoneyFox.Windows.Views
{
    public sealed partial class StatisticMonthlyExpensesView
    {
        public StatisticMonthlyExpensesView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ExpensePlotView.Model = null;
            base.OnNavigatingFrom(e);
        }

        private async void SetDate(object sender, RoutedEventArgs e)
        {
            await new SelectDateRangeDialog().ShowAsync();
        }
    }
}