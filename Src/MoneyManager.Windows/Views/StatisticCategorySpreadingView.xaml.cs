using System;
using Windows.UI.Xaml;
using MoneyManager.Core.ViewModels;
using MoneyManager.Windows.Dialogs;

namespace MoneyManager.Windows.Views
{
    public sealed partial class StatisticCategorySpreadingView : IDisposable
    {
        public StatisticCategorySpreadingView()
        {
            InitializeComponent();
        }

        private async void SetDate(object sender, RoutedEventArgs e)
        {
            await new SelectDateRangeDialog().ShowAsync();
            (DataContext as StatisticCategorySpreadingViewModel)?.LoadCommand.Execute();
        }

        public void Dispose()
        {
            SpreadingPlotView.Model = null;
        }
    }
}