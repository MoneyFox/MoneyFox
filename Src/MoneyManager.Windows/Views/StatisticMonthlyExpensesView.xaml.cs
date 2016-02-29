using System;
using Windows.UI.Xaml;
using MoneyManager.Windows.Views.Dialogs;

namespace MoneyManager.Windows.Views
{
    public sealed partial class StatisticMonthlyExpensesView : IDisposable
    {
        public StatisticMonthlyExpensesView()
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
            (DataContext as StatisticMonthlyExpensesViewModel)?.LoadCommand.Execute();
        }
    }
}