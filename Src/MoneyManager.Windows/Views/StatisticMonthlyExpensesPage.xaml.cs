using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using MoneyManager.Core.ViewModels;
using MoneyManager.Windows.Dialogs;

namespace MoneyManager.Windows.Views
{
    public sealed partial class StatisticMonthlyExpensesPage : IDisposable
    {
        public StatisticMonthlyExpensesPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ExpensePlotView.Model = (DataContext as StatisticMonthlyExpensesViewModel)?.MonthlyExpensesModel;
        }

        private async void SetDate(object sender, RoutedEventArgs e)
        {
            await new SelectDateRangeDialog().ShowAsync();
            (DataContext as StatisticCashFlowViewModel)?.LoadCommand.Execute();
        }

        public void Dispose()
        {
            ExpensePlotView.Model = null;
        }
    }
}
